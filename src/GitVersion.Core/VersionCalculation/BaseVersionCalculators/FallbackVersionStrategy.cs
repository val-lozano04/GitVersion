using GitVersion.Common;
using GitVersion.Model.Configuration;

namespace GitVersion.VersionCalculation;

/// <summary>
/// Version is 0.1.0.
/// BaseVersionSource is the "root" commit reachable from the current commit.
/// Does not increment.
/// </summary>
public class FallbackVersionStrategy : VersionStrategyBaseWithInheritSupport
{
    public FallbackVersionStrategy(IRepositoryStore repositoryStore, Lazy<GitVersionContext> versionContext)
        : base(repositoryStore, versionContext)
    {
    }

    public override IEnumerable<BaseVersion> GetVersions(IBranch branch, EffectiveConfiguration configuration)
    {
        if (Context.CurrentBranch.Tip == null)
            throw new GitVersionException("No commits found on the current branch.");

        yield return new BaseVersion("Fallback base version", true, new SemanticVersion(), null, null);
    }
}
