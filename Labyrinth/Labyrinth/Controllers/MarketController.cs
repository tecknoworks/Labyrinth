using Labyrinth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Labyrinth.Controllers
{
    public class MarketController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Market
        public ActionResult Index()
        {
            var buyer = new Guid(User.Identity.GetUserId());
            return View(db.Sells.ToList().OrderBy(item => item.ItemId).Where( item => item.IsSold == false && item.PlayerId != buyer));
        }

        public ActionResult buyItem(int id)
        {

            var buyer = new Guid(User.Identity.GetUserId());
            Sell sell = db.Sells.Find(id);
            if (db.Players.Find(buyer).Gold >= sell.Price)
            {
                sell.IsSold = true;
                sell.Player.Gold += sell.Price;
                db.Players.Find(buyer).Gold -= sell.Price;

                List<PlayerItem> playerItem = db.PlayerItems.Where(x => x.ItemId == sell.ItemId && x.PlayerId == sell.PlayerId).ToList();
                if (playerItem.Count() != 0)
                {
                    foreach (var pi in playerItem)
                    {
                        pi.Quantity += sell.Quantity;
                    }
                }
                else
                {
                    PlayerItem newPlayerItem = new PlayerItem();
                    newPlayerItem.ItemId = sell.ItemId;
                    newPlayerItem.PlayerId = buyer;
                    newPlayerItem.Quantity = sell.Quantity;
                    newPlayerItem.Item = sell.Item;
                    newPlayerItem.Player = db.Players.Find(buyer);
                    db.PlayerItems.Add(newPlayerItem);
                }
                db.SaveChanges();
                return View("Index", db.Sells.ToList().OrderBy(item => item.ItemId).Where(item => item.IsSold == false && item.PlayerId != buyer));
            }
            else
            {
                return View("NoGold");
            }
        }
        public ActionResult sellItems()
        {
            var buyer = new Guid(User.Identity.GetUserId());
            return View("SellItems", db.PlayerItems.Where(x => x.PlayerId == buyer).ToList());
        }


        [HttpPost]
        public ActionResult sellItem( string itemId, string name, string quantity, string price, string itemQuant)
        {
            try
            {
                var iditem = Int32.Parse(itemId);
                var name1 = name;
                int quant = Int32.Parse(quantity);
                int itemquant = Int32.Parse(itemQuant);
                var seller = new Guid(User.Identity.GetUserId());
                if (quant <= itemquant && quant > 0)
                {
                    int pr = Int32.Parse(price);
                    db.PlayerItems.Where(x => x.PlayerId == seller && x.ItemId == iditem).ToList().ForEach(x => x.Quantity -= quant);
                    Sell sell = new Sell();
                    sell.IsSold = false;
                    sell.Item = db.Items.Find(iditem);
                    sell.ItemId = iditem;
                    sell.Player = db.Players.Find(new Guid(User.Identity.GetUserId()));
                    sell.PlayerId = new Guid(User.Identity.GetUserId());
                    sell.Price = pr;
                    sell.Quantity = quant;
                    db.Sells.Add(sell);
                    db.SaveChanges();
                }
                return View("SellItems", db.PlayerItems.Where(x => x.PlayerId == seller).ToList());

               
            }
            catch
            {
                return View("NoGold");
            }
        }

        public FileContentResult getImage(int id)
        {
            var image = db.Items.Find(id).Image;
            if (image != null)
            {
                return new FileContentResult(image, "image/png");
            }
            else
            {
                return null;
            }
        }
    }
}