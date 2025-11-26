using BusinessLayer.Concrete.ConcreteUow;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraversalCoreProject.Areas.Admin.Models;

namespace TraversalCoreProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(AccountViewModel model)
        {
            var valueSender = _accountService.TGetById(model.SenderID);
            var valueRecever = _accountService.TGetById(model.ReceverID);

            valueSender.Balance -= model.Amount;
            valueRecever.Balance += model.Amount;

            List<Account> modifiedAccounts = new List<Account>()
            {
                valueSender, valueRecever
            };

            return View();
        }
    }
}
