using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    public partial class GroceryListViewModel : BaseViewModel
    {
        
        private readonly GlobalViewModel _globalViewModel;

        public Client Client => _globalViewModel.Client;

        public ObservableCollection<GroceryList> GroceryLists { get; set; }
        private readonly IGroceryListService _groceryListService;

        public GroceryListViewModel(IGroceryListService groceryListService, GlobalViewModel globalmodel) 
        {
            Title = "Boodschappenlijst";
            _groceryListService = groceryListService;
            GroceryLists = new(_groceryListService.GetAll());
            _globalViewModel = globalmodel;
        }

        [RelayCommand]
        public async Task SelectGroceryList(GroceryList groceryList)
        {
            Dictionary<string, object> paramater = new() { { nameof(GroceryList), groceryList } };
            await Shell.Current.GoToAsync($"{nameof(Views.GroceryListItemsView)}?Titel={groceryList.Name}", true, paramater);
        }
        public override void OnAppearing()
        {
            base.OnAppearing();
            GroceryLists = new(_groceryListService.GetAll());
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            GroceryLists.Clear();
        }
        [RelayCommand]
        public void ShowBoughtProducts()
        {
            var globalmodel = _globalViewModel;

            if (globalmodel.Client.userRole == Client.Role.Admin)
            {

                Shell.Current.GoToAsync(nameof(Views.BoughtProductsView));
            }
        }
    }
}
