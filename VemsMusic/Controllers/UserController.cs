﻿using Marten;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using VemsMusic.Models;
using VemsMusic.Other_Data.Interfaces;
using VemsMusic.Other_Data.ViewModels;

namespace VemsMusic.Controllers
{
    [Authorize(Roles = "admin, user")]
    public class UserController : Controller
    {
        private readonly IAllMusic _allMusic;
        private readonly IAllUsers _allUsers;

        public UserController(IAllMusic allMusic, IAllUsers allUsers)
        {
            _allMusic = allMusic;
            _allUsers = allUsers;
        }

        [Route("~/User/MyMusic")]
        public IActionResult MyMusic()
        {
            string userId = HttpContext.Request.Cookies["id"];
            var user = _allUsers.GetUserById(Convert.ToInt32(userId));

            if (user.Musics.IsEmpty())
            {
                return Redirect("~/Home/NoItems/Музыка не добавлена");
            }

            var musicObj = new MusicViewModel();
            var musicList = new List<Music>();

            foreach (var item in user.Musics)
            {
                musicList.Add(_allMusic.GetMusicsById(item.Id));
            }

            musicObj.AllMusic = musicList;
            return View(musicObj);
        }

        [Route("~/AddMusicToUser/{musicId}")]
        public IActionResult AddMusicToUser(int musicId)
        {
            string userId = HttpContext.Request.Cookies["id"];
            _allUsers.AddMusicToUser(musicId, Convert.ToInt32(userId));

            return Redirect("~/");
        }
    }
}
