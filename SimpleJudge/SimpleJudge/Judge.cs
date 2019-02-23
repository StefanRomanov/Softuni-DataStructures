using System;
using System.Collections.Generic;
using System.Linq;

public class Judge : IJudge
{
    private readonly HashSet<int> users;
    private readonly HashSet<int> contests;
    private readonly Dictionary<int,Submission> idMap;

    public Judge()
    {
        contests = new HashSet<int>();
        users = new HashSet<int>();
        idMap = new Dictionary<int, Submission>();
    }

    public void AddContest(int contestId)
    {
        if (!contests.Contains(contestId))
        {
            contests.Add(contestId);
        }
    }

    public void AddSubmission(Submission submission)
    {
        if (contests.Contains(submission.ContestId) && users.Contains(submission.UserId))
        {
            if (!idMap.ContainsKey(submission.Id))
            {
                idMap.Add(submission.Id,submission);
            }
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public void AddUser(int userId)
    {
        if (!users.Contains(userId))
        {
            users.Add(userId);
        }
    }

    public void DeleteSubmission(int submissionId)
    {
        if (!idMap.ContainsKey(submissionId))
        {
            throw new InvalidOperationException();
        }

        idMap.Remove(submissionId);
    }

    public IEnumerable<Submission> GetSubmissions()
    {
        return idMap.Values.OrderBy(x => x);
    }

    public IEnumerable<int> GetUsers()
    {
        return users.OrderBy(x => x);
    }

    public IEnumerable<int> GetContests()
    {
        return contests.OrderBy(x => x);;
    }

    public IEnumerable<Submission> SubmissionsWithPointsInRangeBySubmissionType(int minPoints, int maxPoints, SubmissionType submissionType)
    {
        return idMap.Values
            .Where(x => x.Points >= minPoints && x.Points <= maxPoints && x.Type == submissionType);
    }

    public IEnumerable<int> ContestsByUserIdOrderedByPointsDescThenBySubmissionId(int userId)
    {
        return idMap.Values
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.Points)
            .ThenBy(x => x.Id)
            .Select(x => x.ContestId)
            .Distinct();
    }

    public IEnumerable<Submission> SubmissionsInContestIdByUserIdWithPoints(int points, int contestId, int userId)
    {
        if (!contests.Contains(contestId))
        {
            throw new InvalidOperationException();
        }
        
        var result = idMap.Values
            .Where(x => x.Points == points && x.UserId == userId && x.ContestId == contestId);

        if (!result.Any())
        {
            throw new InvalidOperationException();
        }

        return result;
    }

    public IEnumerable<int> ContestsBySubmissionType(SubmissionType submissionType)
    {
        return idMap.Values
            .Where(s => s.Type == submissionType)
            .Select(s => s.ContestId)
            .Distinct();
    }
}