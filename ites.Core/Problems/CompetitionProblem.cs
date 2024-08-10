using ites.Core.Exeptions;

namespace ites.Core.Problems
{
    public static class CompetitionProblem
    {
        public static readonly ApiException NotFound =
            new("Конкурс не найден", 404);
    }
}
