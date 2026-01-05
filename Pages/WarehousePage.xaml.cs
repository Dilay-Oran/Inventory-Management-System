using Inventory_Management_System.Data;
using Inventory_Management_System.Models;
using Microsoft.VisualBasic;

namespace Inventory_Management_System.Pages;

public partial class WarehousePage : ContentPage
{
    private WarehouseDB  warehousedb = new WarehouseDB();
	public WarehousePage()
	{
		InitializeComponent();
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshListView();
    }

    private async void SaveButton_OnClicked(object? sender, EventArgs e) // chatgpt
    {
        if (string.IsNullOrWhiteSpace(Capacity.Text))
        {
            await DisplayAlert("Error", "Please enter a capacity value.", "OK");
            return;
        }

        bool isNumber = int.TryParse(Capacity.Text, out int result);

        if (isNumber)
        {
            await warehousedb.CreateAsync(Destination.Text, result);

            Destination.Text = string.Empty;
            Capacity.Text = string.Empty;

            await RefreshListView();
        }
        else
        {
            await DisplayAlert("Invalid Input", "You should enter an integer to capacity!", "OK");
            Capacity.Text = string.Empty;
        }
    }
     

    public async void DeleteButton_OnClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var item = (WarehouseItem)button.CommandParameter;
        await warehousedb.DeleteAsync(item);
        await warehousedb.DeleteAsync(item);
        await RefreshListView();
    }

    private async Task RefreshListView()
    {
        TasksListView.ItemsSource = null;
        TasksListView.ItemsSource = await warehousedb.GetAllAsync();
    }
}