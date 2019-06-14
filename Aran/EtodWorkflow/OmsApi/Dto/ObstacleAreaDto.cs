using System;

namespace OmsApi.Dto
{
    public class ObstacleAreaDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsChecked { get; set; }
    }
}