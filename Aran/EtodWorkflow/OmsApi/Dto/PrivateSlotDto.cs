﻿using OmsApi.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Dto
{
    public class PrivateSlotDto : IBaseDto
    {
        public string Name { get; set; }

        public DateTime CreationDate { get; set; }

        public SlotStatus Status { get; set; }

        public long Id { get; set; }
    }
}