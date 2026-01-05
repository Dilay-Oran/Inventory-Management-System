using Inventory_Management_System.Data;
using Inventory_Management_System.Models;

namespace Inventory_Management_System.Pages;

public partial class DutiesPage : ContentPage
{
   
    
        private DutiesDB dutiesdb = new DutiesDB();

        public DutiesPage()
        {
            InitializeComponent();
    
            RefreshListView();
            ;


        }
        private async void AddButton_OnClicked(object? sender, EventArgs e)
        {
            //FakeDb.AddToDo(Title.Text, DueDate.Date);
            await dutiesdb.CreateAsync(Duty.Text, DueDate.Date);
            Duty.Text = string.Empty;
            DueDate.Date=DateTime.Now;
            await RefreshListView();
        }
        private async void SaveButton_OnClicked(object? sender, EventArgs e)
        {
            await dutiesdb.CreateAsync(Duty.Text, DueDate.Date);
            Duty.Text = string.Empty;
            DueDate.Date = DateTime.Now;
            await RefreshListView();
        }
        private async Task RefreshListView()
        {
            TasksListView.ItemsSource = null;
            TasksListView.ItemsSource = await dutiesdb.GetAllAsync();
        }
        private async void CheckBox_OnClicked(object sender, CheckedChangedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var task = (DutiesItem)checkBox.BindingContext;

            if (task != null)
            {
                await dutiesdb.CompletionStatusAsync(task);
                TasksListView.ItemsSource = await dutiesdb.GetNotCompletedAsync();
            }
        }



}