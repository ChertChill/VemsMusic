﻿using System.Collections.Generic;
using VemsMusic.Models;

namespace VemsMusic.Other_Data.Interfaces
{
    public interface IAllMusic
    {
        IEnumerable<Music> GetAllMusic { get; }
        void DeleteMusic(int id);
        void AddMusic(Music music);
        void UpdateMusic(Music music);
    }
}
