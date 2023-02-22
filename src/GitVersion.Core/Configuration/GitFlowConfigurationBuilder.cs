using GitVersion.Extensions;
using GitVersion.VersionCalculation;

namespace GitVersion.Configuration;

internal sealed class GitFlowConfigurationBuilder : ConfigurationBuilderBase<GitFlowConfigurationBuilder>
{
    public static GitFlowConfigurationBuilder New => new();

    private GitFlowConfigurationBuilder()
    {
        WithConfiguration(new()
        {
            AssemblyFileVersioningScheme = AssemblyFileVersioningScheme.MajorMinorPatch,
            AssemblyVersioningScheme = AssemblyVersioningScheme.MajorMinorPatch,
            CommitDateFormat = "yyyy-MM-dd",
            MajorVersionBumpMessage = IncrementStrategyFinder.DefaultMajorPattern,
            MinorVersionBumpMessage = IncrementStrategyFinder.DefaultMinorPattern,
            NoBumpMessage = IncrementStrategyFinder.DefaultNoBumpPattern,
            PatchVersionBumpMessage = IncrementStrategyFinder.DefaultPatchPattern,
            SemanticVersionFormat = SemanticVersionFormat.Strict,
            LabelPrefix = GitVersionConfiguration.DefaultLabelPrefix,
            LabelPreReleaseWeight = 60000,
            UpdateBuildNumber = true,
            VersioningMode = VersioningMode.ContinuousDelivery,
            Regex = string.Empty,
            Label = "{BranchName}",
            Increment = IncrementStrategy.Inherit,
            CommitMessageIncrementing = CommitMessageIncrementMode.Enabled,
            PreventIncrementOfMergedBranchVersion = false,
            TrackMergeTarget = false,
            TrackMergeMessage = true,
            TracksReleaseBranches = false,
            IsReleaseBranch = false,
            IsMainline = false
        });

        WithBranch(DevelopBranch.Name).WithConfiguration(new()
        {
            Increment = IncrementStrategy.Minor,
            Regex = DevelopBranch.RegexPattern,
            VersioningMode = VersioningMode.ContinuousDeployment,
            SourceBranches = new HashSet<string>(),
            Label = "alpha",
            PreventIncrementOfMergedBranchVersion = false,
            TrackMergeTarget = true,
            TracksReleaseBranches = true,
            IsMainline = false,
            IsReleaseBranch = false,
            PreReleaseWeight = 0
        });

        WithBranch(MainBranch.Name).WithConfiguration(new()
        {
            Increment = IncrementStrategy.Patch,
            Regex = MainBranch.RegexPattern,
            SourceBranches = new HashSet<string> {
                DevelopBranch.Name,
                ReleaseBranch.Name
            },
            Label = string.Empty,
            PreventIncrementOfMergedBranchVersion = true,
            TrackMergeTarget = false,
            TracksReleaseBranches = false,
            IsMainline = true,
            IsReleaseBranch = false,
            PreReleaseWeight = 55000
        });

        WithBranch(ReleaseBranch.Name).WithConfiguration(new()
        {
            Increment = IncrementStrategy.None,
            Regex = ReleaseBranch.RegexPattern,
            SourceBranches = new HashSet<string> {
                DevelopBranch.Name,
                MainBranch.Name,
                SupportBranch.Name,
                ReleaseBranch.Name
            },
            Label = "beta",
            PreventIncrementOfMergedBranchVersion = true,
            TrackMergeTarget = false,
            TracksReleaseBranches = false,
            IsMainline = false,
            IsReleaseBranch = true,
            PreReleaseWeight = 30000
        });

        WithBranch(FeatureBranch.Name).WithConfiguration(new()
        {
            Increment = IncrementStrategy.Inherit,
            Regex = FeatureBranch.RegexPattern,
            VersioningMode = VersioningMode.ContinuousDelivery,
            SourceBranches = new HashSet<string> {
                DevelopBranch.Name,
                MainBranch.Name,
                ReleaseBranch.Name,
                FeatureBranch.Name,
                SupportBranch.Name,
                HotfixBranch.Name
            },
            Label = "{BranchName}",
            PreReleaseWeight = 30000
        });

        WithBranch(PullRequestBranch.Name).WithConfiguration(new()
        {
            Increment = IncrementStrategy.Inherit,
            Regex = PullRequestBranch.RegexPattern,
            VersioningMode = VersioningMode.ContinuousDelivery,
            SourceBranches = new HashSet<string> {
                DevelopBranch.Name,
                MainBranch.Name,
                ReleaseBranch.Name,
                FeatureBranch.Name,
                SupportBranch.Name,
                HotfixBranch.Name
            },
            Label = "PullRequest",
            LabelNumberPattern = @"[/-](?<number>\d+)",
            PreReleaseWeight = 30000
        });

        WithBranch(HotfixBranch.Name).WithConfiguration(new()
        {
            Increment = IncrementStrategy.Inherit,
            Regex = HotfixBranch.RegexPattern,
            VersioningMode = VersioningMode.ContinuousDelivery,
            SourceBranches = new HashSet<string> {
                ReleaseBranch.Name,
                MainBranch.Name,
                SupportBranch.Name,
                HotfixBranch.Name
            },
            Label = "beta",
            PreReleaseWeight = 30000
        });

        WithBranch(SupportBranch.Name).WithConfiguration(new()
        {
            Increment = IncrementStrategy.Patch,
            Regex = SupportBranch.RegexPattern,
            SourceBranches = new HashSet<string> { MainBranch.Name },
            Label = string.Empty,
            PreventIncrementOfMergedBranchVersion = true,
            TrackMergeTarget = false,
            TracksReleaseBranches = false,
            IsMainline = true,
            IsReleaseBranch = false,
            PreReleaseWeight = 55000
        });

        WithBranch(UnknownBranch.Name).WithConfiguration(new()
        {
            Regex = UnknownBranch.RegexPattern,
            Label = "{BranchName}",
            VersioningMode = VersioningMode.ContinuousDelivery,
            Increment = IncrementStrategy.Inherit,
            SourceBranches = new HashSet<string> {
                MainBranch.Name,
                DevelopBranch.Name,
                ReleaseBranch.Name,
                FeatureBranch.Name,
                PullRequestBranch.Name,
                HotfixBranch.Name,
                SupportBranch.Name
            }
        });
    }

    public static readonly BranchMetaData MainBranch = new()
    {
        Name = "main",
        RegexPattern = "^master$|^main$"
    };

    public static readonly BranchMetaData DevelopBranch = new()
    {
        Name = "develop",
        RegexPattern = "^dev(elop)?(ment)?$"
    };

    public static readonly BranchMetaData ReleaseBranch = new()
    {
        Name = "release",
        RegexPattern = "^releases?[/-]"
    };

    public static readonly BranchMetaData FeatureBranch = new()
    {
        Name = "feature",
        RegexPattern = "^features?[/-]"
    };

    public static readonly BranchMetaData PullRequestBranch = new()
    {
        Name = "pull-request",
        RegexPattern = @"^(pull|pull\-requests|pr)[/-]"
    };

    public static readonly BranchMetaData HotfixBranch = new()
    {
        Name = "hotfix",
        RegexPattern = "^hotfix(es)?[/-]"
    };

    public static readonly BranchMetaData SupportBranch = new()
    {
        Name = "support",
        RegexPattern = "^support[/-]"
    };

    public static readonly BranchMetaData UnknownBranch = new()
    {
        Name = "unknown",
        RegexPattern = ".*"
    };
}