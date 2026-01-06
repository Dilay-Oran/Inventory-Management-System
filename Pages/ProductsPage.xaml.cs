using Inventory_Management_System.Data;
using Inventory_Management_System.Models;
using Microsoft.VisualBasic;

namespace Inventory_Management_System.Pages;

public partial class ProductsPage : ContentPage
{
	public ProductsPage()
	{
		InitializeComponent();
	}
    private ProductsDB productsdb = new ProductsDB();
   
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await RefreshListView();
    }

    private async void SaveButton_OnClicked(object? sender, EventArgs e) // chatgpt
    {
        var productsitem = new ProductsItem { ProductsName = ProductName.Text }; //gpt
        await productsdb.CreateAsync(productsitem);
        ProductName.Text = string.Empty;
        await RefreshListView();
  
    }


    public async void DeleteButton_OnClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var item = (ProductsItem)button.CommandParameter;
        await productsdb.DeleteAsync(item);
        await RefreshListView();
    }

    private async Task RefreshListView()
    {
        TasksListView.ItemsSource = null;
        TasksListView.ItemsSource = await productsdb.GetAllAsync();
    }
}