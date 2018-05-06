﻿using PlaySpace.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PlaySpace.Controllers
{
    [Authorize(Roles="admin")]
    public class AdminController : Controller
    {
        UserContext context = new UserContext();

        public ViewResult Index(int id)
        {
            List<Game> gamelist = new List<Game>();
            foreach(Game game in context.Games)
            {
                if (game.CategoryId == id)
                {
                    gamelist.Add(game);
                }
            }
            return View(gamelist);
        }

        public ViewResult Edit(int gameId)
        {
            Game game = context.Games
                .FirstOrDefault(g => g.GameId == gameId);
            return View(game);
        }

        [HttpPost]
        public ActionResult Edit(Game game, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    game.ImageMimeType = image.ContentType;
                    game.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(game.ImageData, 0, image.ContentLength);
                }                
                context.SaveGame(game);
                TempData["message"] = string.Format("Изменения в игре \"{0}\" были сохранены", game.Name);
                return RedirectToAction("Index", new { id=game.CategoryId });
            }
            else
            {
                return View(game);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Game());
        }

        public ActionResult Delete(int gameId)
        {
            Game deletedGame = context.DeleteGame(gameId);
            if (deletedGame != null)
            {
                TempData["message"] = string.Format("Игра \"{0}\" была удалена",
                    deletedGame.Name);
            }
            return RedirectToAction("Index", new { id=deletedGame.CategoryId});
        }
    }
}