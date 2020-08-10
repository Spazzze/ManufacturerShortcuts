using AtomicTorch.CBND.CoreMod.ClientComponents.Input;
using AtomicTorch.CBND.GameApi.Scripting;

namespace ManufacturerShortcuts.Scripts.ManufacturerShortcuts
{
    public class ManufacturerShortcutsBootstrapperClient : BaseBootstrapper
    {
        public override void ClientInitialize()
        {
            ClientInputManager.RegisterButtonsEnum<ManufacturerShortcutsButton>();
        }
    }
}