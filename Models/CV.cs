namespace JobWebsite.Models
{
    public class CV
    {
        public int Id { get; set; }
        public virtual Candidate? Candidate { get; set; }
        public virtual Job? Job { get; set; }
    }
}
