﻿using System.Collections.Generic;
using System.Linq;
using VemsMusic.Models;
using VemsMusic.Other_Data.Interfaces;

namespace VemsMusic.Other_Data.Repositories
{
    public class MusicRepository : IAllMusic
    {
        private readonly AppDBContext _dbContext;

        public MusicRepository(AppDBContext appDBContext)
        {
            _dbContext = appDBContext;
        }

        public IEnumerable<Music> GetAllMusic
        {
            get
            {
                return _dbContext.Musics.ToList();
            }
        }

        public Music GetMusicsById(int id)
        {
            return _dbContext.Find<Music>(id);
        }

        public void DeleteMusic(int id)
        {
            _dbContext.Musics.Remove(_dbContext.Musics.Find(id));
            _dbContext.SaveChanges();
        }

        public void AddMusic(Music music)
        {
            _dbContext.Musics.Add(music);
            _dbContext.SaveChanges();
        }

        public void UpdateMusic(Music music)
        {
            _dbContext.Musics.Update(music);
            _dbContext.SaveChanges();
        }
    }
}
