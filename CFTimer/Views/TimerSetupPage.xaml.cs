using CFTimer.ViewModels;

namespace CFTimer.Views;

public partial class TimerSetupPage : ContentPage
{
    private readonly TimerSetupViewModel _vm;

    public TimerSetupPage(TimerSetupViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    private void OnIncrementRounds(object? sender, EventArgs e) => _vm.TotalRounds = Math.Min(_vm.TotalRounds + 1, 100);
    private void OnDecrementRounds(object? sender, EventArgs e) => _vm.TotalRounds = Math.Max(_vm.TotalRounds - 1, 1);
    private void OnIncrementPrep(object? sender, EventArgs e) => _vm.PrepSeconds = Math.Min(_vm.PrepSeconds + 5, 60);
    private void OnDecrementPrep(object? sender, EventArgs e) => _vm.PrepSeconds = Math.Max(_vm.PrepSeconds - 5, 0);
}
