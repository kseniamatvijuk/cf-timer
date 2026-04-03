using CFTimer.ViewModels;

namespace CFTimer.Views;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsViewModel _vm;

    public SettingsPage(SettingsViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    private void OnIncrementPrep(object? sender, EventArgs e) => _vm.DefaultPrepSeconds = Math.Min(_vm.DefaultPrepSeconds + 5, 60);
    private void OnDecrementPrep(object? sender, EventArgs e) => _vm.DefaultPrepSeconds = Math.Max(_vm.DefaultPrepSeconds - 5, 0);
}
