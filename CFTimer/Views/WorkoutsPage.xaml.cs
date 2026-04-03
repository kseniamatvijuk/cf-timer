using CFTimer.ViewModels;

namespace CFTimer.Views;

public partial class WorkoutsPage : ContentPage
{
    private readonly WorkoutsViewModel _vm;

    public WorkoutsPage(WorkoutsViewModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.LoadDataCommand.Execute(null);
    }

    private void OnFilterTapped(object? sender, EventArgs e)
    {
        if (sender is Button btn)
            _vm.SelectedFilter = btn.Text;
    }
}
