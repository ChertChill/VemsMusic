﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VemsMusic.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        public int? RoleId { get; set; }
        public Role Role { get; set; }
        public int? MusicId { get; set; }
        public List<Music> Musics { get; set; } = new();

    }
}
