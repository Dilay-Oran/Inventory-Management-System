using Inventory_Management_System.Data;
using Inventory_Management_System.Models;

namespace Inventory_Management_System.Pages;

public partial class DutiesPage : ContentPage
{
   
    
        private DutiesDB db = new DutiesDB();

        public DutiesPage()
        {
            InitializeComponent();
    
            RefreshListView();
            ;


        }
        private async void AddButton_OnClicked(object? sender, EventArgs e)
        {
            //FakeDb.AddToDo(Title.Text, DueDate.Date);
            await db.CreateAsync(Duty.Text, DueDate.Date);
            Duty.Text = string.Empty;
            DueDate.Date=DateTime.Now;
            await RefreshListView();
        }
        private async void SaveButton_OnClicked(object? sender, EventArgs e)
        {
            await db.CreateAsync(Duty.Text, DueDate.Date);
            Duty.Text = string.Empty;
            DueDate.Date = DateTime.Now;
            await RefreshListView();
        }
        private async Task RefreshListView()
        {
            TasksListView.ItemsSource = null;
            TasksListView.ItemsSource = await db.GetAllAsync();
        }
        private async void TasksListView_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as DutiesItem;
        await db.CompletionStatusAsync(item!);
            await RefreshListView();

        }
    }