using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class GroceryListItemsService : IGroceryListItemsService
    {
        private readonly IGroceryListItemsRepository _groceriesRepository;
        private readonly IProductRepository _productRepository;

        public GroceryListItemsService(IGroceryListItemsRepository groceriesRepository, IProductRepository productRepository)
        {
            _groceriesRepository = groceriesRepository;
            _productRepository = productRepository;
        }

        public List<GroceryListItem> GetAll()
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public List<GroceryListItem> GetAllOnGroceryListId(int groceryListId)
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll().Where(g => g.GroceryListId == groceryListId).ToList();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public GroceryListItem Add(GroceryListItem item)
        {
            return _groceriesRepository.Add(item);
        }

        public GroceryListItem? Delete(GroceryListItem item)
        {
            throw new NotImplementedException();
        }

        public GroceryListItem? Get(int id)
        {
            return _groceriesRepository.Get(id);
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            return _groceriesRepository.Update(item);
        }

        public List<BestSellingProducts> GetBestSellingProducts(int topX = 5)
        {
            var groceriesrepo = _groceriesRepository;
            var productrepo = _productRepository;
            var groceryItems = groceriesrepo.GetAll();

            Dictionary<int, int> bestproductspair = new Dictionary<int, int>();

          
            foreach (var groceryItem in groceryItems)
            {
                var productId = groceryItem.ProductId;

                if (!bestproductspair.ContainsKey(productId))
                {
                    bestproductspair[productId] = 0;
                }

                bestproductspair[productId] += groceryItem.Amount; 
            }

            
            var ordered = bestproductspair.OrderByDescending(n => n.Value).Take(topX);

            
            Dictionary<int, int> ranking = new Dictionary<int, int>();
            int count = 1;
            foreach (var item in ordered)
            {
                ranking[item.Key] = count;
                count++;
            }

            
            List<BestSellingProducts> bestproducts = new List<BestSellingProducts>();
            foreach (var item in ordered)
            {
                var product = productrepo.Get(item.Key);

                int rank = ranking[item.Key];
                BestSellingProducts bestproduct = new BestSellingProducts(
                    item.Key,
                    product.Name,
                    product.Stock,
                    item.Value, 
                    rank
                );

                bestproducts.Add(bestproduct);
            }

            return bestproducts;
        }



        private void FillService(List<GroceryListItem> groceryListItems)
        {
            foreach (GroceryListItem g in groceryListItems)
            {
                g.Product = _productRepository.Get(g.ProductId) ?? new(0, "", 0);
            }
        }
    }
}
