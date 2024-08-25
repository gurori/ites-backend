using ites.Core.Exeptions;

namespace ites.Core.Problems
{
    public static class TeamProblem
    {
        public static readonly ApiException NotFound =
            new("Команда не найдена", 404);
    }
}
