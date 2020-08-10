using System;
using System.Collections.Generic;
using System.Linq;
using AtomicTorch.CBND.CoreMod.Characters.Player;
using AtomicTorch.CBND.CoreMod.ClientComponents.Input;
using AtomicTorch.CBND.CoreMod.SoundPresets;
using AtomicTorch.CBND.CoreMod.Systems.Notifications;
using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
using AtomicTorch.CBND.CoreMod.UI.Controls.Game.Items.Controls;
using AtomicTorch.CBND.CoreMod.UI.Controls.Game.Items.Managers;
using AtomicTorch.CBND.GameApi.Data.Characters;
using AtomicTorch.CBND.GameApi.Data.Items;
using AtomicTorch.CBND.GameApi.Scripting;
using AtomicTorch.CBND.GameApi.ServicesClient;
using ManufacturerShortcuts.Scripts.ManufacturerShortcuts;

namespace AtomicTorch.CBND.CoreMod.UI.Controls.Game.WorldObjects.Manufacturers.Data.Base
{
    public class ViewModelManufacturerExchange : BaseViewModel
    {
        private static ICharacter Character => Api.Client.Characters.CurrentPlayerCharacter;
        private readonly List<IClientItemsContainer> inputContainers = new List<IClientItemsContainer>();
        private readonly List<IClientItemsContainer> outputContainers = new List<IClientItemsContainer>();
        private static readonly IItemsClientService ItemsService = Api.Client.Items;
        private static readonly Lazy<string> CurrentCharacterName = new Lazy<string>(Api.Client.Characters.CurrentPlayerCharacter.Name);
        private readonly bool isInsertingWithoutMatch;
        private ClientInputContext inputListener;
        private bool isActive;

        public ViewModelManufacturerExchange(
            IEnumerable<IItemsContainer> outputContainersList,
            IReadOnlyCollection<IItemsContainer> inputContainersList = null,
            bool insertIfInputEmpty = false)
        {
            outputContainers.AddRange(outputContainersList.Select(it => it as IClientItemsContainer));
            if (inputContainersList != null) inputContainers.AddRange(inputContainersList.Select(it => it as IClientItemsContainer));
            IsActive = !ItemsContainerExchangeControl.IsEnabledOnlyWhenLoaded;
            isInsertingWithoutMatch = insertIfInputEmpty;
            if (isInsertingWithoutMatch && VeryLazyGuysList.IsLazyGuy(CurrentCharacterName.Value)) ExecuteCommandTakeAllAndMatchUp();
        }

        private bool IsActive
        {
            set
            {
                if (isActive == value) return;

                isActive = value;
                NotifyThisPropertyChanged();

                if (isActive)
                {
                    inputListener = ClientInputContext
                        .Start("Container exchange")
                        .HandleButtonDown(GameButton.ContainerTakeAll, ExecuteCommandTakeAll)
                        .HandleButtonDown(GameButton.ContainerMoveItemsMatchDown, ExecuteCommandMatchDown)
                        .HandleButtonDown(GameButton.ContainerMoveItemsMatchUp, ExecuteCommandMatchUp)
                        .HandleButtonDown(ManufacturerShortcutsButton.ExecuteCommandTakeAllAndPutAll, ExecuteCommandTakeAllAndMatchUp);
                }
                else
                {
                    ClientContainersExchangeManager.Unregister(this);
                    inputListener.Stop();
                    inputListener = null;
                }
            }
        }

        private void ExecuteCommandMatch(bool isUp)
        {
            var playerPrivateState = PlayerCharacter.GetPrivateState(Character);
            var playerInventory = playerPrivateState.ContainerInventory;
            var playerHotbar = playerPrivateState.ContainerHotbar;


            var receivingContainers = new List<IItemsContainer>();
            IEnumerable<IItem> sourceItems;

            if (isUp)
            {
                // move items "up" - from player inventory to this crate container
                sourceItems = playerInventory.Items;
                receivingContainers.AddRange(inputContainers);
                foreach (var receivingContainer in receivingContainers)
                    ClientContainerSortHelper.ConsolidateItemStacks((IClientItemsContainer) receivingContainer);
            }
            else
            {
                // move items "down" - from this crate container to player containers
                sourceItems = outputContainers.SelectMany(i => i.Items);
                receivingContainers.Add(playerInventory);
                receivingContainers.Add(playerHotbar);
            }

            var isAtLeastOneItemMoved = false;

            if (isUp && isInsertingWithoutMatch)
            {
                foreach (var itemToMove in sourceItems.OrderBy(i => i.ProtoItem.Id))
                {
                    if (receivingContainers
                        .Any(it => ItemsService.MoveOrSwapItem(itemToMove, it, allowSwapping: false, isLogErrors: false)))
                        isAtLeastOneItemMoved = true;
                }

                if (isAtLeastOneItemMoved) ItemsSoundPresets.ItemGeneric.PlaySound(ItemSound.Drop);
                return;
            }

            var itemTypesToMove = new HashSet<IProtoItem>(receivingContainers.SelectMany(i => i.Items).Select(i => i.ProtoItem));

            var itemsToMove = sourceItems
                .Where(item => itemTypesToMove.Contains(item.ProtoItem))
                .OrderBy(i => i.ProtoItem.Id)
                .ToList();

            foreach (var itemToMove in itemsToMove)
            {
                if (receivingContainers
                    .Any(it => ItemsService.MoveOrSwapItem(itemToMove, it, allowSwapping: false, isLogErrors: false)))
                    isAtLeastOneItemMoved = true;
            }

            if (isAtLeastOneItemMoved) ItemsSoundPresets.ItemGeneric.PlaySound(ItemSound.Drop);

            if (!isUp && itemsToMove.Any(i => outputContainers.Contains(i.Container)))
            {
                // at least one item stuck in the container when matching down
                // it means there are not enough space
                NotificationSystem.ClientShowNotificationNoSpaceInInventory();
            }
        }

        private void ExecuteCommandTakeAllAndMatchUp()
        {
            ExecuteCommandTakeAll();
            ExecuteCommandMatch(true);
        }

        private void ExecuteCommandTakeAll()
        {
            ExecuteCommandMatchDown();
            var character = Api.Client.Characters.CurrentPlayerCharacter;
            foreach (var container in outputContainers)
                character.ProtoCharacter.ClientTryTakeAllItems(character, container, showNotificationIfInventoryFull: true);
        }

        private void ExecuteCommandMatchDown() => ExecuteCommandMatch(false);

        private void ExecuteCommandMatchUp() => ExecuteCommandMatch(true);

        protected override void DisposeViewModel()
        {
            IsActive = false;
            inputListener?.Stop();
            inputListener = null;

            base.DisposeViewModel();
        }
    }
}