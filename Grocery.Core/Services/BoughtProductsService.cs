
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class BoughtProductsService : IBoughtProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGroceryListRepository _groceryListRepository;
        public BoughtProductsService(IGroceryListItemsRepository groceryListItemsRepository, IGroceryListRepository groceryListRepository, IClientRepository clientRepository, IProductRepository productRepository)
        {
            _groceryListItemsRepository=groceryListItemsRepository;
            _groceryListRepository=groceryListRepository;
            _clientRepository=clientRepository;
            _productRepository=productRepository;
        }
        public List<BoughtProducts> Get(int? productId)
        {
            List<BoughtProducts> delijst = new List<BoughtProducts>();
            var grocelist_repo = _groceryListRepository;
            var groceitem_repo = _groceryListItemsRepository;
            var product_repo = _productRepository;
            var client_repo = _clientRepository;

            var product = product_repo.Get((int)productId);
            var allgroceitems = groceitem_repo.GetAll();
            foreach (var item in allgroceitems)
            {
                if (item.ProductId == productId)
                {
                    
                    var groceryList = _groceryListRepository.Get(item.GroceryListId);

                    
                    var client = _clientRepository.Get(groceryList.ClientId);

                    
                    var boughtProduct = new BoughtProducts(client, groceryList, product);

                    
                    delijst.Add(boughtProduct);
                }
            }


            return delijst;
        }
    }
}
