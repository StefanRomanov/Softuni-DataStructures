using System;

public class Submission : IComparable<Submission>
{
    public int Id { get; set; }

    public int Points { get; set; }

    public SubmissionType Type { get; set; }

    public int ContestId { get; set; }

    public int UserId { get; set; }

    public Submission(int id, int points, SubmissionType type, int contestId, int userId)
    {
        Id = id;
        Points = points;
        Type = type;
        ContestId = contestId;
        UserId = userId;
    }

    public int CompareTo(Submission other)
    {
        return Id.CompareTo(other.Id);
    }
}
