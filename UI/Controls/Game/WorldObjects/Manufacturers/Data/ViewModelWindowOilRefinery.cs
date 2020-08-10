using System.Collections.Generic;
using AtomicTorch.CBND.CoreMod.StaticObjects.Structures.Manufacturers;
using AtomicTorch.CBND.CoreMod.Systems.Crafting;
using AtomicTorch.CBND.CoreMod.Systems.LiquidContainer;
using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
using AtomicTorch.CBND.CoreMod.UI.Controls.Game.WorldObjects.Data;
using AtomicTorch.CBND.CoreMod.UI.Controls.Game.WorldObjects.Manufacturers.Data.Base;
using AtomicTorch.CBND.GameApi.Data.Items;
using AtomicTorch.CBND.GameApi.Data.State;
using AtomicTorch.CBND.GameApi.Data.World;

namespace AtomicTorch.CBND.CoreMod.UI.Controls.Game.WorldObjects.Manufacturers.Data
{
    public class ViewModelWindowOilRefinery : BaseViewModel
    {
        private readonly ObjectManufacturerPublicState manufacturerPublicState;
        private ViewModelManufacturerExchange viewModelManufacturerExchange;

        public ViewModelWindowOilRefinery(
            IStaticWorldObject worldObject,
            ManufacturingState manufacturingState,
            ManufacturingState manufacturingStateProcessedGasoline,
            ManufacturingState manufacturingStateProcessedMineralOil,
            ManufacturingConfig manufacturingConfig,
            ManufacturingConfig manufacturingConfigProcessedGasoline,
            ManufacturingConfig manufacturingConfigProcessedMineralOil,
            LiquidContainerState liquidStateRawPetroleum,
            LiquidContainerState liquidStateProcessedGasoline,
            LiquidContainerState liquidStateProcessedMineralOil,
            LiquidContainerConfig liquidConfigRawPetroleum,
            LiquidContainerConfig liquidConfigProcessedGasoline,
            LiquidContainerConfig liquidConfigProcessedMineralOil)
        {
            this.WorldObjectManufacturer = worldObject;

            this.ViewModelManufacturingStateRawPetroleum = new ViewModelManufacturingState(
                worldObject,
                manufacturingState,
                manufacturingConfig);

            this.ViewModelManufacturingStateProcessedGasoline = new ViewModelManufacturingState(
                worldObject,
                manufacturingStateProcessedGasoline,
                manufacturingConfigProcessedGasoline);

            this.ViewModelManufacturingStateProcessedMineralOil = new ViewModelManufacturingState(
                worldObject,
                manufacturingStateProcessedMineralOil,
                manufacturingConfigProcessedMineralOil);

            this.ViewModelLiquidStateRawPetroleum = new ViewModelLiquidContainerState(
                liquidStateRawPetroleum,
                liquidConfigRawPetroleum);

            this.ViewModelLiquidStateProcessedGasoline = new ViewModelLiquidContainerState(
                liquidStateProcessedGasoline,
                liquidConfigProcessedGasoline);

            this.ViewModelLiquidStateProcessedMineralOil = new ViewModelLiquidContainerState(
                liquidStateProcessedMineralOil,
                liquidConfigProcessedMineralOil);
            // prepare active state property
            this.manufacturerPublicState = worldObject.GetPublicState<ObjectManufacturerPublicState>();
            this.manufacturerPublicState.ClientSubscribe(_ => _.IsActive,
                _ => NotifyPropertyChanged(nameof(IsManufacturingActive)), this);
            viewModelManufacturerExchange = new ViewModelManufacturerExchange(
                new List<IItemsContainer>
                {
                    manufacturingState.ContainerOutput,
                    manufacturingStateProcessedGasoline.ContainerOutput,
                    manufacturingStateProcessedMineralOil.ContainerOutput
                },
                new List<IItemsContainer>
                {
                    manufacturingState.ContainerInput,
                    manufacturingStateProcessedGasoline.ContainerInput,
                    manufacturingStateProcessedMineralOil.ContainerInput
                },
                true);
        }

        public ViewModelWindowOilRefinery()
        {
        }

        public bool IsManufacturingActive => this.manufacturerPublicState.IsActive;

        public ViewModelLiquidContainerState ViewModelLiquidStateProcessedGasoline { get; }

        public ViewModelLiquidContainerState ViewModelLiquidStateProcessedMineralOil { get; }

        public ViewModelLiquidContainerState ViewModelLiquidStateRawPetroleum { get; }

        public ViewModelManufacturingState ViewModelManufacturingStateProcessedGasoline { get; }

        public ViewModelManufacturingState ViewModelManufacturingStateProcessedMineralOil { get; }

        public ViewModelManufacturingState ViewModelManufacturingStateRawPetroleum { get; }

        public IStaticWorldObject WorldObjectManufacturer { get; }
    }
}