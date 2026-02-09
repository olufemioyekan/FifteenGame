# .NET 10 Upgrade Plan

## Table of Contents

- [Executive Summary](#executive-summary)
- [Migration Strategy](#migration-strategy)
- [Detailed Dependency Analysis](#detailed-dependency-analysis)
- [Project-by-Project Plans](#project-by-project-plans)
  - [AI.FifteenGame](#aififteengame)
  - [AI.FifteenGame.Console](#aififteengameconsole)
- [Package Update Reference](#package-update-reference)
- [Breaking Changes Catalog](#breaking-changes-catalog)
- [Risk Management](#risk-management)
- [Testing & Validation Strategy](#testing--validation-strategy)
- [Complexity & Effort Assessment](#complexity--effort-assessment)
- [Source Control Strategy](#source-control-strategy)
- [Success Criteria](#success-criteria)

---

## Executive Summary

### Scenario Description
Upgrade the FifteenGame solution from .NET Framework 4.8 to .NET 10.0. This involves converting two legacy projects to SDK-style format and updating their target frameworks.

### Scope

| Metric | Value |
|--------|-------|
| Total Projects | 3 |
| Projects Requiring Upgrade | 2 |
| Projects Already Compatible | 1 (AI.FifteenGame.WebApi) |
| Total Lines of Code | 1,282 |
| NuGet Packages | 11 (all compatible) |
| Security Vulnerabilities | 0 |

### Current State ? Target State

| Project | Current | Target | SDK Conversion Required |
|---------|---------|--------|------------------------|
| AI.FifteenGame | net48 | net10.0 | ? Yes |
| AI.FifteenGame.Console | net48 | net10.0 | ? Yes |
| AI.FifteenGame.WebApi | net10.0 | net10.0 | No (already SDK-style) |

### Selected Strategy

**All-At-Once Strategy** - All projects upgraded simultaneously in a single atomic operation.

**Rationale**:
- Small solution with only 2 projects requiring upgrade
- Simple linear dependency structure (no circular dependencies)
- Low total codebase size (1,282 LOC)
- All NuGet packages are compatible with .NET 10
- No security vulnerabilities to address
- No API breaking changes detected

### Complexity Classification

**Simple Solution** - Based on discovered metrics:
- ?5 projects (3 total, 2 to upgrade) ?
- Dependency depth ?2 (linear chain) ?
- No high-risk projects ?
- No security vulnerabilities ?

### Iteration Strategy
Fast batch approach: 2-3 detail iterations covering all projects together.

### Critical Issues
None identified. All packages compatible, no breaking changes detected.

---

## Migration Strategy

### Approach Selection

**Selected: All-At-Once Strategy**

This solution meets all criteria for simultaneous upgrade:
- ? Small solution (<5 projects)
- ? All projects on .NET Framework 4.8 (homogeneous)
- ? Simple dependency structure
- ? Low external dependency complexity
- ? All packages compatible with target framework

### Strategy Rationale

The All-At-Once approach is optimal because:
1. **No intermediate states needed** - With only 2 projects to upgrade, there's no benefit to phased migration
2. **Clean dependency resolution** - All projects move to .NET 10 together
3. **Fastest completion** - Single atomic operation minimizes total effort
4. **Reduced complexity** - No multi-targeting or compatibility shims required

### Execution Approach

**Single Atomic Operation**:
1. Convert both legacy projects to SDK-style format
2. Update all target frameworks to net10.0
3. Remove packages that are now included in the framework
4. Restore dependencies
5. Build entire solution
6. Fix any compilation errors (none expected based on assessment)
7. Validate solution builds with 0 errors

### Dependency-Based Ordering

Within the atomic operation, projects are processed in this order:
1. **AI.FifteenGame** (leaf node - no project dependencies)
2. **AI.FifteenGame.Console** (depends on AI.FifteenGame)

This ensures the shared library is available when dependent projects are updated.

### Parallel vs Sequential

**Sequential within atomic operation** - Due to the dependency relationship, AI.FifteenGame must be converted before AI.FifteenGame.Console to maintain project reference integrity.

---

## Detailed Dependency Analysis

### Dependency Graph Summary

```
AI.FifteenGame (Library - Leaf Node)
    ?
    ??? AI.FifteenGame.Console (Console App)
    ??? AI.FifteenGame.WebApi (Web API - Already .NET 10)
```

### Project Classification

| Project | Type | Dependencies | Dependants | Migration Phase |
|---------|------|--------------|------------|-----------------|
| AI.FifteenGame | ClassLibrary | 0 | 2 | Phase 1 (Leaf) |
| AI.FifteenGame.Console | ConsoleApp | 1 | 0 | Phase 1 (Atomic) |
| AI.FifteenGame.WebApi | AspNetCore | 1 | 0 | N/A (Already .NET 10) |

### Critical Path

1. **AI.FifteenGame** must be upgraded first as it is the shared library
2. **AI.FifteenGame.Console** depends on AI.FifteenGame
3. **AI.FifteenGame.WebApi** already targets .NET 10 and will benefit from the upgraded library

### Circular Dependencies
None detected.

### Migration Grouping

**All-At-Once Batch** (Single Atomic Operation):
- AI.FifteenGame (convert to SDK-style + update TFM)
- AI.FifteenGame.Console (convert to SDK-style + update TFM + remove redundant packages)

Both projects will be converted and upgraded together in a single coordinated operation. The conversion tool handles SDK-style conversion, and then all target framework and package updates occur atomically.

---

## Project-by-Project Plans

### AI.FifteenGame

#### Current State
- **Target Framework**: net48
- **Project Type**: ClassicClassLibrary (non-SDK style)
- **Dependencies**: 0 project references
- **Dependants**: 2 (AI.FifteenGame.Console, AI.FifteenGame.WebApi)
- **NuGet Packages**: 0
- **Lines of Code**: 1,109
- **Risk Level**: ?? Low

#### Target State
- **Target Framework**: net10.0
- **Project Type**: SDK-style ClassLibrary
- **Updated Packages**: None required

#### Migration Steps

1. **SDK-Style Conversion**
   - Convert `AI.FifteenGame.csproj` from legacy format to SDK-style
   - Tool: `upgrade_convert_project_to_sdk_style`
   - This automatically handles:
     - Removing unnecessary elements (AssemblyInfo generation)
     - Updating project structure
     - Preserving project references

2. **Target Framework Update**
   - Change `<TargetFramework>` from `net48` to `net10.0`

3. **Package Updates**
   - No NuGet packages to update

4. **Expected Breaking Changes**
   - None identified based on assessment
   - No API compatibility issues detected

5. **Code Modifications**
   - None expected
   - Standard .NET APIs used are compatible

6. **Validation**
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] Dependent projects can reference successfully

---

### AI.FifteenGame.Console

#### Current State
- **Target Framework**: net48
- **Project Type**: ClassicDotNetApp (non-SDK style)
- **Dependencies**: 1 (AI.FifteenGame)
- **Dependants**: 0
- **NuGet Packages**: 10
- **Lines of Code**: 122
- **Risk Level**: ?? Low

#### Target State
- **Target Framework**: net10.0
- **Project Type**: SDK-style Console Application
- **Updated Packages**: Remove 5 packages (now included in framework)

#### Migration Steps

1. **SDK-Style Conversion**
   - Convert `AI.FifteenGame.Console.csproj` from legacy format to SDK-style
   - Tool: `upgrade_convert_project_to_sdk_style`

2. **Target Framework Update**
   - Change `<TargetFramework>` from `net48` to `net10.0`

3. **Package Updates**
   - **Remove** (functionality included in .NET 10):
     - `System.Buffers` (4.6.1)
     - `System.Memory` (4.6.3)
     - `System.Numerics.Vectors` (4.6.1)
     - `System.Threading.Tasks.Extensions` (4.6.3)
     - `System.ValueTuple` (4.6.1)
   - **Keep** (still required):
     - `Microsoft.Bcl.AsyncInterfaces` (10.0.2)
     - `System.IO.Pipelines` (10.0.2)
     - `System.Runtime.CompilerServices.Unsafe` (6.1.2)
     - `System.Text.Encodings.Web` (10.0.2)
     - `System.Text.Json` (10.0.2)

4. **Expected Breaking Changes**
   - None identified based on assessment

5. **Code Modifications**
   - None expected

6. **Validation**
   - [ ] Project builds without errors
   - [ ] Project builds without warnings
   - [ ] Application runs correctly

---

## Package Update Reference

### Packages to Remove (Included in .NET 10 Framework)

These packages are now part of the .NET 10 runtime and should be removed from AI.FifteenGame.Console:

| Package | Current Version | Action | Reason |
|---------|-----------------|--------|--------|
| System.Buffers | 4.6.1 | ? Remove | Included in framework |
| System.Memory | 4.6.3 | ? Remove | Included in framework |
| System.Numerics.Vectors | 4.6.1 | ? Remove | Included in framework |
| System.Threading.Tasks.Extensions | 4.6.3 | ? Remove | Included in framework |
| System.ValueTuple | 4.6.1 | ? Remove | Included in framework |

### Packages to Retain (Compatible)

These packages are compatible with .NET 10 and should remain:

| Package | Version | Project | Status |
|---------|---------|---------|--------|
| Microsoft.Bcl.AsyncInterfaces | 10.0.2 | AI.FifteenGame.Console | ? Compatible |
| System.IO.Pipelines | 10.0.2 | AI.FifteenGame.Console | ? Compatible |
| System.Runtime.CompilerServices.Unsafe | 6.1.2 | AI.FifteenGame.Console | ? Compatible |
| System.Text.Encodings.Web | 10.0.2 | AI.FifteenGame.Console | ? Compatible |
| System.Text.Json | 10.0.2 | AI.FifteenGame.Console | ? Compatible |
| Microsoft.AspNetCore.OpenApi | 10.0.2 | AI.FifteenGame.WebApi | ? Compatible |

### Package Summary by Project

| Project | Packages Before | Packages After | Change |
|---------|-----------------|----------------|--------|
| AI.FifteenGame | 0 | 0 | No change |
| AI.FifteenGame.Console | 10 | 5 | -5 (framework packages removed) |
| AI.FifteenGame.WebApi | 1 | 1 | No change (already .NET 10) |

---

## Breaking Changes Catalog

### Framework Breaking Changes

**None Identified**

The assessment detected no API compatibility issues between .NET Framework 4.8 and .NET 10.0 for this codebase.

### Package Breaking Changes

**None Identified**

All retained packages are at versions compatible with .NET 10.

### Potential Areas to Monitor

While no breaking changes were detected, the following areas should be verified during testing:

| Area | Description | Verification |
|------|-------------|--------------|
| Console I/O | Console application behavior | Run AI.FifteenGame.Console and verify output |
| Game Logic | Core game algorithms in AI.FifteenGame | Verify game solving produces correct results |
| Web API Integration | AI.FifteenGame.WebApi references the library | Build and verify API endpoints work |

### Known .NET Framework to .NET Migration Considerations

While not detected as issues in this codebase, be aware of:
- **AppDomain changes** - Single AppDomain in .NET Core/5+
- **Reflection changes** - Some reflection APIs differ
- **Configuration** - app.config vs appsettings.json (Console app may need review)

These are documented for awareness but assessment indicates no code changes required.

---

## Risk Management

### Risk Assessment Summary

| Project | Risk Level | Rationale |
|---------|------------|-----------|
| AI.FifteenGame | ?? Low | Small codebase, no packages, no breaking changes |
| AI.FifteenGame.Console | ?? Low | Small codebase, all packages compatible |

### Identified Risks

| Risk | Probability | Impact | Mitigation |
|------|-------------|--------|------------|
| SDK conversion issues | Low | Medium | Use built-in conversion tool; manual adjustment if needed |
| Runtime behavior differences | Low | Medium | Test game logic thoroughly after upgrade |
| Package reference resolution | Low | Low | Restore and build will surface issues immediately |

### Security Vulnerabilities

**None identified** - All NuGet packages are free of known security vulnerabilities.

### Contingency Plans

**If SDK conversion fails:**
- Manually create SDK-style project files
- Reference the original files using glob patterns
- Preserve all existing functionality

**If build errors occur:**
- Address compilation errors in dependency order
- Consult .NET migration documentation for specific API changes
- Assessment shows 0 expected issues, so this is unlikely

**If runtime issues occur:**
- Compare behavior against .NET Framework version
- Check for platform-specific code paths
- Review Console I/O and file system operations

---

## Testing & Validation Strategy

### Phase 1: Build Validation (After Atomic Upgrade)

**Objective**: Verify all projects compile successfully

| Checkpoint | Criteria | Method |
|------------|----------|--------|
| Solution Build | 0 errors | `dotnet build AI.FifteenGame.sln` |
| Warning Review | Review any warnings | Build output analysis |
| Package Restore | All packages resolve | `dotnet restore` success |

### Phase 2: Functional Validation

**Objective**: Verify application functionality

| Test | Description | Expected Result |
|------|-------------|-----------------|
| Console App Launch | Run AI.FifteenGame.Console | Application starts without errors |
| Game Logic | Execute game solving algorithm | Correct solution output |
| Web API Build | Verify WebApi still builds | Successful compilation |

### Validation Checklist

#### AI.FifteenGame
- [ ] SDK-style conversion complete
- [ ] Target framework updated to net10.0
- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] Referenced by dependent projects successfully

#### AI.FifteenGame.Console
- [ ] SDK-style conversion complete
- [ ] Target framework updated to net10.0
- [ ] Redundant packages removed
- [ ] Project builds without errors
- [ ] Project builds without warnings
- [ ] Application runs correctly

#### Solution-Wide
- [ ] Full solution builds with 0 errors
- [ ] No package dependency conflicts
- [ ] AI.FifteenGame.WebApi still builds (validates library compatibility)

---

## Complexity & Effort Assessment

### Per-Project Complexity

| Project | Complexity | Dependencies | Packages | LOC | Risk Factors |
|---------|------------|--------------|----------|-----|--------------|
| AI.FifteenGame | ?? Low | 0 | 0 | 1,109 | None |
| AI.FifteenGame.Console | ?? Low | 1 | 10?5 | 122 | Package cleanup |

### Overall Complexity Assessment

**Solution Complexity: Low**

- Simple linear dependency chain
- No circular dependencies
- All packages compatible
- No breaking changes detected
- Small total codebase

### Migration Phases

| Phase | Operations | Complexity |
|-------|------------|------------|
| Atomic Upgrade | SDK conversion + TFM update + package cleanup | ?? Low |
| Validation | Build + functional test | ?? Low |

### Resource Requirements

| Skill | Level Required |
|-------|----------------|
| .NET Development | Basic |
| SDK-style Projects | Basic familiarity |
| Package Management | Basic |

---

## Source Control Strategy

### Branch Strategy

| Branch | Purpose |
|--------|---------|
| `Innovation-Prep` | Source branch (current) |
| `Innovation-Prep` | Upgrade work branch (same as source per user preference) |

### Commit Strategy

**Single Commit Approach** (Recommended for All-At-Once)

Since this is an atomic upgrade of a small solution, all changes should be committed together:

```
feat: Upgrade AI.FifteenGame and AI.FifteenGame.Console to .NET 10

- Convert AI.FifteenGame to SDK-style project format
- Convert AI.FifteenGame.Console to SDK-style project format  
- Update target frameworks from net48 to net10.0
- Remove framework-included packages from Console project
```

### Commit Checkpoints

| Checkpoint | When | Contents |
|------------|------|----------|
| Atomic Upgrade Complete | After all conversions and TFM updates | All project changes |

### Review Criteria

Before merge/completion:
- [ ] All projects build successfully
- [ ] No new warnings introduced
- [ ] Functional validation passed

---

## Success Criteria

### Technical Criteria

| Criterion | Measurement |
|-----------|-------------|
| All projects migrated | AI.FifteenGame and AI.FifteenGame.Console target net10.0 |
| SDK-style conversion | Both projects use SDK-style format |
| Package updates applied | Redundant packages removed from Console project |
| Solution builds | 0 errors on `dotnet build` |
| No regressions | AI.FifteenGame.WebApi still builds and references library |

### Quality Criteria

| Criterion | Measurement |
|-----------|-------------|
| No new warnings | Build produces same or fewer warnings |
| Code unchanged | No functional code modifications required |
| Documentation updated | Plan reflects actual changes made |

### Process Criteria

| Criterion | Measurement |
|-----------|-------------|
| All-At-Once strategy followed | Single atomic operation for all upgrades |
| Dependency order respected | AI.FifteenGame converted before AI.FifteenGame.Console |
| Validation completed | All checklist items verified |

### Definition of Done

The .NET 10 upgrade is complete when:

1. ? `AI.FifteenGame.csproj` is SDK-style targeting net10.0
2. ? `AI.FifteenGame.Console.csproj` is SDK-style targeting net10.0
3. ? Framework-included packages removed from Console project
4. ? Full solution builds with 0 errors
5. ? AI.FifteenGame.WebApi continues to build (validates library compatibility)
6. ? Changes committed to source control
