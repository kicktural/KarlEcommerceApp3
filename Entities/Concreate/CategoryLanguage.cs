﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concreate
{
    public class CategoryLanguage : IEntity
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string LangCode { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
