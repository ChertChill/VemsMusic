﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private IEnumerable<Music> GetAllMusic
        {
            get
            {
                var musics = _dbContext.Musics.ToList();
                _dbContext.Genres.Include(g => g.Musics).ToList();
                _dbContext.Groups.Include(g => g.Musics).ToList();
                return musics;
            }
        }

        public Task<IEnumerable<Music>> GetAllMusicAsync()
        {
            return Task.Run(() => GetAllMusic);
        }
        public async Task<Music> GetMusicsByIdAsync(int musicId)
        {
            var musics = await _dbContext.FindAsync<Music>(musicId);
            _dbContext.Genres.Include(g => g.Musics).ToList();
            _dbContext.Groups.Include(g => g.Musics).ToList();
            return musics;
        }
        public async Task AddMusicAsync(Music music)
        {
            _dbContext.Musics.Add(music);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateMusicAsync(Music music)
        {
            var newGenre = _dbContext.Find<Genre>(Convert.ToInt32(music.GenreId));
            _dbContext.Genres.Include(g => g.Musics).ToList();
            var editableMusic = _dbContext.Find<Music>(music.Id);

            editableMusic.Name = music.Name;
            editableMusic.Text = music.Text;
            editableMusic.ImagePath = music.ImagePath;
            editableMusic.AudioPath = music.AudioPath;
            editableMusic.Group = _dbContext.Find<MusicalGroup>(Convert.ToInt32(music.GroupId));
            editableMusic.AdditionDateAndTime = DateTime.Now.ToString("MM.dd.yyyy  HH:mm:ss");

            _dbContext.Musics.Update(editableMusic);

            if (editableMusic.Genres == null)
            {
                editableMusic.Genres = new List<Genre> { newGenre };
            }
            else
            {
                if (editableMusic.Genres.Contains(newGenre))
                {
                    editableMusic.Genres.Remove(newGenre);
                }

                editableMusic.Genres.Add(newGenre);
            }

            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteMusicsGenreAsync(int MusicId, int GenreId)
        {
            var music = await _dbContext.Musics.FindAsync(MusicId);
            var genre = await _dbContext.Genres.FindAsync(GenreId);
            _dbContext.Genres.Include(g => g.Musics).ToList();
            music.Genres.Remove(genre);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteMusicAsync(int musicId)
        {
            _dbContext.Musics.Remove(_dbContext.Musics.Find(musicId));
            await _dbContext.SaveChangesAsync();
        }
    }
}
