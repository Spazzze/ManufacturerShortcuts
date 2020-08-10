using System.ComponentModel;
using AtomicTorch.CBND.CoreMod.ClientComponents.Input;
using AtomicTorch.CBND.GameApi;
using AtomicTorch.CBND.GameApi.ServicesClient;

namespace ManufacturerShortcuts.Scripts.ManufacturerShortcuts
{
    [NotPersistent]
    public enum ManufacturerShortcutsButton
    {
        [Description("Take All and Match items up")] [ButtonInfo(InputKey.Q, Category = "ManufacturerShortcuts")]
        ExecuteCommandTakeAllAndPutAll,
    }
}