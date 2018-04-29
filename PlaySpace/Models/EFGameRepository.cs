﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlaySpace.Models
{
    public class EFGameRepository : IGameRepository
    {
        UserContext context = new UserContext();
        public Game DeleteGame(int gameId)
        {
            Game dbEntry = context.Games.Find(gameId);
            if (dbEntry != null)
            {
                context.Games.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public IEnumerable<Game> Games
        {
            get { return context.Games; }
        }
        public void SaveGame(Game game)
        {
            if (game.GameId == 0)
                context.Games.Add(game);
            else
            {
                Game dbEntry = context.Games.Include(nameof(Game.Keys)).FirstOrDefault(g => g.GameId == game.GameId);
                if (dbEntry != null)
                {
                    dbEntry.Name = game.Name;
                    dbEntry.Discription = game.Discription;
                    dbEntry.Price = game.Price;
                    dbEntry.Discount = game.Discount;
                    dbEntry.ImageData = game.ImageData;
                    dbEntry.ImageMimeType = game.ImageMimeType;
                    dbEntry.Category = game.Category;
                    if ((dbEntry.Keys.FirstOrDefault(p=>p.Item == game.ActiveKey) == null)&&(dbEntry.ActiveKey!=game.ActiveKey))
                    {
                        dbEntry.Keys.Add(new Key
                        {
                            Item = game.ActiveKey,
                            GameId = game.GameId
                        });
                    }
                    
                }
            }
            context.SaveChanges();
        }
    }
}