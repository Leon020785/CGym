namespace CGym.Domain.Entities
{
    public class Activity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartTime { get; set; }

        public int Capacity { get; set; }

        public int TrainerId { get; set; }

        public Trainer Trainer { get; set; }
    }
}