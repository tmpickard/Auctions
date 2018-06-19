using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Auctions.Models;
using Microsoft.AspNetCore.Http;

namespace Auctions.Controllers
{
    public class HomeController : Controller
    {
        private AuctionContext _context;
        public HomeController(AuctionContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("register")]
        public IActionResult Register(registerViewModel model)
        {
            var CheckUsername = _context.User.SingleOrDefault(User => User.Username == model.Username);
            if(CheckUsername != null)
            {
                return RedirectToAction("Index");
            }
            if(ModelState.IsValid)
            {
                User newUser = new User{
                    Firstname = model.Firstname,
                    Lastname = model.Lastname,
                    Username = model.Username,
                    Password = model.Password,
                    Wallet = 1000.00,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _context.User.Add(newUser);
                _context.SaveChanges();
                newUser = _context.User.SingleOrDefault(User => model.Username == newUser.Username);
                HttpContext.Session.SetInt32("id", newUser.UserId);
                return RedirectToAction("Auction");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login(string Username, string Password)
        {
            var returningUser = _context.User.SingleOrDefault(User => User.Username == Username && User.Password == Password);
            if(returningUser != null)
            {
                HttpContext.Session.SetInt32("id", returningUser.UserId);
                return RedirectToAction("Auction");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("auction")]
        public IActionResult Auction()
        {
            if(HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index");
            }
            var ActiveUser = _context.User.SingleOrDefault(User => User.UserId == HttpContext.Session.GetInt32("id"));
            List<User> AllUsers = _context.User.ToList();
            List<Item> AllItems = _context.Item.Where(Date => Date.EndDate > DateTime.Now).ToList();
            ViewBag.ActiveUser = ActiveUser;
            ViewBag.AllItems = AllItems;
            return View("Auction");
        }
        [HttpGet]
        [Route("NewAuction")]
        public IActionResult NewAuction()
        {
            if(HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index");
            }
            var ActiveUser = _context.User.SingleOrDefault(User => User.UserId == HttpContext.Session.GetInt32("id"));
            ViewBag.ActiveUser = ActiveUser;
            return View("NewAuction");
        }
        [HttpPost]
        [Route("newitem")]
        public IActionResult ItemCheck(auctionViewModel model)
        {
            if(HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index");
            }
            else if(model.EndDate < DateTime.Now || model.StartBid < 0)
            {
                return RedirectToAction("NewAuction");
            }
            if(ModelState.IsValid)
            {
                Item newItem = new Item{
                    UserId = model.UserId,
                    Product = model.Product,
                    StartBid = model.StartBid,
                    EndDate = model.EndDate,
                    Description = model.Description,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _context.Item.Add(newItem);
                _context.SaveChanges();
                return RedirectToAction("Auction");
            }
            return RedirectToAction("NewAuction");
        }
        [HttpGet]
        [Route("auction/item/{ItemId}")]
        public IActionResult AuctionItem(int ItemId)
        {
            var ActiveUser = _context.User.SingleOrDefault(User => User.UserId == HttpContext.Session.GetInt32("id"));
            var CurrentItem = _context.Item.SingleOrDefault(Id => Id.ItemId == ItemId);
            List<User> Items = _context.User.ToList();
            List<User> Bids = _context.User.ToList();
            ViewBag.ActiveUser = ActiveUser;
            ViewBag.CurrentItem = CurrentItem;
            ViewBag.Items = Items;
            return View("AuctionItem");
        }
        [HttpPost]
        [Route("bid")]
        public IActionResult Bid(int UserId, int ItemId, double UserBid)
        {
            var CurrentItem = _context.Item.SingleOrDefault(Id => Id.ItemId == ItemId);
            if(HttpContext.Session.GetInt32("id") == null)
            {
                return RedirectToAction("Index");
            }
            if(UserBid <= CurrentItem.StartBid)
            {
                return RedirectToAction("Auction");
            } else {
                Bid newBid = new Bid{
                    UserId = UserId,
                    ItemId = ItemId,
                    UserBid = UserBid
                };
                var OldBid = _context.Bid.SingleOrDefault(Bid => Bid.ItemId == ItemId);
                if(OldBid != null){
                    _context.Bid.Remove(OldBid);
                    _context.SaveChanges();
                }
                _context.Bid.Add(newBid);
                _context.SaveChanges();
                var changedItem = _context.Item.SingleOrDefault(bid => bid.ItemId == ItemId);
                changedItem.StartBid = UserBid;
                _context.SaveChanges();
                return RedirectToAction("Auction");
            }
        }
        [Route("delete/{ItemId}")]
        public IActionResult Delete(int ItemId)
        {
            List<Bid> BadBids = _context.Bid.Where(Item => Item.ItemId == ItemId).ToList();
            _context.Remove(BadBids);
            _context.SaveChanges();
            var BadItem = _context.Item.SingleOrDefault(Item => Item.ItemId == ItemId);
            _context.Remove(BadItem);
            _context.SaveChanges();
            return RedirectToAction("Auction");
        }
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
