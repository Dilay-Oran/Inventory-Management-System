using Inventory_Management_System.Data;
using Inventory_Management_System.Models;
using Microsoft.VisualBasic;

namespace Inventory_Management_System.Pages;

public partial class WarehousePage : ContentPage
{
    private WarehouseDB  warehoousedb = new WarehouseDB();
	public WarehousePage()
	{
		InitializeComponent();
	}
    private async void SaveButton_OnClicked(object? sender, EventArgs e)
    {
        int capacity = int.TryParse(Capacity.Text, out int result) ? result : 0; // chatgpt

        await warehoousedb.CreateAsync(Destination.Text, capacity);

        Destination.Text = string.Empty;
        Capacity.Text = string.Empty;

        await RefreshListView();
    }


    private async Task RefreshListView()
    {
        TasksListView.ItemsSource = null;
        TasksListView.ItemsSource = await warehoousedb.GetAllAsync();
    }
}