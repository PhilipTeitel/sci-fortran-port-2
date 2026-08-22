<!-- Execution path inventory contract:
- One row per executable path. This is the scoping foundation, not a design doc.
- Main table: active paths only. Suspected-dead, unknown, and retired paths go in the appendix.
- One evidence grade per row. Do not combine grades.
- After Status: Snapshot, do not edit cells to record later decisions. Append Errata or produce vN+1.
- Omit sections that do not apply. Do not write "not applicable" rows.
- No prose preamble that restates table cells. History lives in git.
-->

# Execution Path Inventory

**Legacy repo:** `/Users/philipteitel/code/ADD-migrations/sci-fortran-legacy` (`e586903a26cc50ca8942f20ca3bccbd8814e6252`)
**Version:** `v1`
**Status:** `Snapshot`
**Date:** `2026-08-22`
**Owner:** Archaeologist (`/inventory-paths`)

## Summary

This inventory lists independently callable SciFor/SciFortran library public names and CLI utilities recovered from the legacy clone. **237 paths** were enumerated: **225 active** in the main table and **12** in the appendix (`suspected-dead`, `unknown`, or `retired`). Dominant triggers are Fortran `public ::` library APIs (one row per generic name or operator; overloads of the same generic are one path; parameters and types are omitted), plus `numutils` CLIs on that tree's `Makefile` `all`, the `src/Makefile` `all`/`libscifor.a` build, env/setup scripts, and this clone's fidelity harness. `SQUARE_LATTICE`, `LIST_*`, `PADE`, `VECTORS`, and `SPLINE_FINTER_MOD::finter` compile into `libscifor.a` but are not `USE`d by umbrella `SCIFOR`. Still unknown: whether the ~165 file-scope special-function subroutines in `FUNCTIONS.o` are called via `EXTERNAL`; whether consumers `USE` the non-umbrella modules; which FFT backend is the intended default (`src/FFTGF.f90` currently symlinks to the Numerical Recipes implementation while `scripts/build.sh` can switch); and whether `bin/setup_sf.sh` is still the installer. What comes with a path is in the [dependency graph](dependency-graph.md); the modernization verdict is in the [assessment](ASSESSMENT.md).

---

## 1. Active paths

| ID | Trigger | Description | Evidence |
|----|---------|-------------|----------|
| XP-001 | CLI `src/Makefile` `all` / `libscifor.a` | Compiles `allmod` objects into `libscifor.a` and `libscifor_deb.a`, copies `.mod` files to `$(SFINCLUDE)`, and moves archives to `$(SFLIB)`. | E3 `src/Makefile:4-22` |
| XP-002 | CLI source script `bin/sciforvars.sh` | Sourced (not executed): sets `SFLIB`/`SFINCLUDE`/`SFETC`/`SFBIN`, extends loader paths, and sources `etc/library.conf`. | E3 `bin/sciforvars.sh:1-38`; sourced by `scripts/build.sh:39` |
| XP-003 | library API `COMMON_VARS::version` | Prints SciFor and caller git revisions, writes `version.inc`, and calls `timestamp`. | E3 `src/COMVARS.f90:92,112-124` |
| XP-004 | library API `COMMON_VARS::timestamp` | Writes the current YMDHMS date as a time stamp to a unit (default stdout). | E3 `src/COMVARS.f90:93,136-145` |
| XP-005 | library API `COMMON_VARS::abort` | Public alias of `error`: prints an abort message and `stop`s. | E3 `src/COMVARS.f90:86-88,94` |
| XP-006 | library API `COMMON_VARS::error` | Prints a colored error line (optional MPI rank) and `stop`s. | E3 `src/COMVARS.f90:94,192-209` |
| XP-007 | library API `COMMON_VARS::warning` | Prints a colored warning line (optional MPI rank); does not stop. | E3 `src/COMVARS.f90:94,211-227` |
| XP-008 | library API `COMMON_VARS::msg` | Prints a `msg:` line (optional MPI rank and trailing blank lines). | E3 `src/COMVARS.f90:95,235-254` |
| XP-009 | library API `COMMON_VARS::txtfy` | Converts integer, real, or complex values to a left-aligned character string. | E3 `src/COMVARS.f90:82-84,96,261-284` |
| XP-010 | library API `COMMON_VARS::bold` | Wraps text in ANSI SGR bold. | E3 `src/COMVARS.f90:97,290-294` |
| XP-011 | library API `COMMON_VARS::underline` | Wraps text in ANSI SGR underline. | E3 `src/COMVARS.f90:98,296-300` |
| XP-012 | library API `COMMON_VARS::highlight` | Wraps text in ANSI SGR reverse video. | E3 `src/COMVARS.f90:99,302-306` |
| XP-013 | library API `COMMON_VARS::erased` | Wraps text in ANSI SGR strikethrough. | E3 `src/COMVARS.f90:100,308-312` |
| XP-014 | library API `COMMON_VARS::red` | Wraps text in ANSI bright-red foreground. | E3 `src/COMVARS.f90:101,314-318` |
| XP-015 | library API `COMMON_VARS::green` | Wraps text in ANSI bright-green foreground. | E3 `src/COMVARS.f90:101,320-324` |
| XP-016 | library API `COMMON_VARS::yellow` | Wraps text in ANSI bright-yellow foreground. | E3 `src/COMVARS.f90:101,326-330` |
| XP-017 | library API `COMMON_VARS::blue` | Wraps text in ANSI bright-blue foreground. | E3 `src/COMVARS.f90:101,332-336` |
| XP-018 | library API `COMMON_VARS::purple` | Wraps text in ANSI bright-purple foreground. | E3 `src/COMVARS.f90:101,338-342` |
| XP-019 | library API `COMMON_VARS::cyan` | Wraps text in ANSI bright-cyan foreground. | E3 `src/COMVARS.f90:101,344-348` |
| XP-020 | library API `COMMON_VARS::bold_red` | Wraps text in ANSI bold bright-red foreground. | E3 `src/COMVARS.f90:102,350-354` |
| XP-021 | library API `COMMON_VARS::bold_green` | Wraps text in ANSI bold bright-green foreground. | E3 `src/COMVARS.f90:102,356-360` |
| XP-022 | library API `COMMON_VARS::bold_yellow` | Wraps text in ANSI bold bright-yellow foreground. | E3 `src/COMVARS.f90:102,362-366` |
| XP-023 | library API `COMMON_VARS::bold_blue` | Wraps text in ANSI bold bright-blue foreground. | E3 `src/COMVARS.f90:102,368-372` |
| XP-024 | library API `COMMON_VARS::bold_purple` | Wraps text in ANSI bold bright-purple foreground. | E3 `src/COMVARS.f90:102,374-378` |
| XP-025 | library API `COMMON_VARS::bold_cyan` | Wraps text in ANSI bold bright-cyan foreground. | E3 `src/COMVARS.f90:102,380-384` |
| XP-026 | library API `COMMON_VARS::bg_red` | Wraps text in ANSI red background. | E3 `src/COMVARS.f90:103,386-390` |
| XP-027 | library API `COMMON_VARS::bg_green` | Wraps text in ANSI green background. | E3 `src/COMVARS.f90:103,392-396` |
| XP-028 | library API `COMMON_VARS::bg_yellow` | Wraps text in ANSI yellow background. | E3 `src/COMVARS.f90:103,398-402` |
| XP-029 | library API `COMMON_VARS::bg_blue` | Wraps text in ANSI blue background. | E3 `src/COMVARS.f90:103,404-408` |
| XP-030 | library API `COMMON_VARS::bg_purple` | Wraps text in ANSI purple background. | E3 `src/COMVARS.f90:103,410-414` |
| XP-031 | library API `COMMON_VARS::bg_cyan` | Wraps text in ANSI cyan background. | E3 `src/COMVARS.f90:103,416-420` |
| XP-032 | library API `PARSE_CMD::parse_cmd_variable` | Reads `NAME=value` command-line arguments into an integer, real, character, or logical variable (optional default and alias name). | E3 `src/PARSECMD.f90:18-23,109-128` |
| XP-033 | library API `PARSE_CMD::get_cmd_variable` | Returns the i-th command-line token split at `=` into uppercased name and value. | E3 `src/PARSECMD.f90:24,87-100` |
| XP-034 | library API `PARSE_CMD::parse_cmd_help` | If argv contains `--help`/`-h`/`help`/`info`, prints a help buffer and `stop`s (or sets optional status). | E3 `src/PARSECMD.f90:25,36-61` |
| XP-035 | library API `PARSE_CMD::print_cmd_help` | Prints a help buffer and `stop`s. | E3 `src/PARSECMD.f90:26,70-78` |
| XP-036 | library API `TIMER::print_bar` | Draws a stdout progress bar for step `i` of `imax`, optionally with ETA. | E3 `src/TIMER.f90:35`; `src/timer_bar.f90:6-25` |
| XP-037 | library API `TIMER::start_timer` | Pushes a nested wall-clock timer using `date_and_time`. | E3 `src/TIMER.f90:36`; `src/timer_chrono.f90:5-18` |
| XP-038 | library API `TIMER::stop_timer` | Pops the timer stack and prints elapsed time. | E3 `src/TIMER.f90:36`; `src/timer_chrono.f90:31-45` |
| XP-039 | library API `TIMER::eta` | Prints expected-time-of-arrival progress to a unit or file. | E3 `src/TIMER.f90:37`; `src/timer_chrono.f90:126-157` |
| XP-040 | library API `IOFILE::file_size` | Returns file size in KiB via `fstat` (0 if missing). | E3 `src/IOFILE.f90:31,102-118` |
| XP-041 | library API `IOFILE::file_length` | Counts non-comment, non-blank lines; may `gunzip` a `.gz` sibling first. | E3 `src/IOFILE.f90:32,174-199` |
| XP-042 | library API `IOFILE::file_info` | Prints `fstat` fields for a file to stdout. | E3 `src/IOFILE.f90:33,131-161` |
| XP-043 | library API `IOFILE::data_open` | If `filename.gz` exists and `filename` does not, runs `gunzip` on it. | E3 `src/IOFILE.f90:34,249-268` |
| XP-044 | library API `IOFILE::data_store` | `gzip`s a file when its size exceeds `store_size` (default 2048 KiB). | E3 `src/IOFILE.f90:35,219-232` |
| XP-045 | library API `IOFILE::set_store_size` | Sets the KiB threshold used by `data_store`. | E3 `src/IOFILE.f90:36,234-238` |
| XP-046 | library API `IOFILE::reg_filename` | Returns `trim(adjustl(trim(file)))`. | E3 `src/IOFILE.f90:37,49-53` |
| XP-047 | library API `IOFILE::reg` | Public alias of `reg_filename`. | E3 `src/IOFILE.f90:15-17,37` |
| XP-048 | library API `IOFILE::txtfit` | Public alias of `reg_filename`. | E3 `src/IOFILE.f90:19-21,37` |
| XP-049 | library API `IOFILE::txtcut` | Public alias of `reg_filename`. | E3 `src/IOFILE.f90:23-25,37` |
| XP-050 | library API `IOFILE::create_data_dir` | Runs `mkdir -v` for a directory name (default `DATAsrc`). | E3 `src/IOFILE.f90:38,281-292` |
| XP-051 | library API `IOFILE::create_dir` | Public alias of `create_data_dir`. | E3 `src/IOFILE.f90:27-29,38` |
| XP-052 | library API `IOFILE::close_file` | Appends a blank line to a named file and closes it. | E3 `src/IOFILE.f90:39,77-82` |
| XP-053 | library API `IOFILE::get_filename` | Returns the path component after the last `/`. | E3 `src/IOFILE.f90:40,55-64` |
| XP-054 | library API `IOFILE::get_filepath` | Returns the directory prefix through the last `/`. | E3 `src/IOFILE.f90:41,66-75` |
| XP-055 | library API `SLPLOT::splot` | Writes 0-d/1-d/2-d/3-d integer, real, or complex arrays to a plot file (many type overloads, one generic). | E3 `src/SLPLOT.f90:12-25,41` |
| XP-056 | library API `SLPLOT::splot3d` | Writes 3-d real or complex data (optional animation overloads) to a plot file. | E3 `src/SLPLOT.f90:28-31,42` |
| XP-057 | library API `SLPLOT::store_data` | Writes 1-d/2-d/3-d integer, real, or complex arrays as stored data files. | E3 `src/SLPLOT.f90:34-43` |
| XP-058 | library API `SLREAD::sread` | Reads 0-d/1-d/2-d/3-d integer, real, or complex arrays from a file (many type overloads, one generic). | E3 `src/SLREAD.f90:13-29,41` |
| XP-059 | library API `SLREAD::read_data` | Reads 1-d or 2-d integer, real, or complex arrays from a stored data file. | E3 `src/SLREAD.f90:31-42` |
| XP-060 | library API `TOOLS::start_loop` | Prints a bold loop banner and starts the timer (MPI rank 0). | E3 `src/TOOLS.f90:14,121-140` |
| XP-061 | library API `TOOLS::end_loop` | Prints a closing banner and stops the timer (MPI rank 0). | E3 `src/TOOLS.f90:15,147-158` |
| XP-062 | library API `TOOLS::linspace` | Returns `num` evenly spaced reals from `start` to `stop` (optional endpoint flags and mesh). | E3 `src/TOOLS.f90:18`; `src/tools_grids.f90:1-29` |
| XP-063 | library API `TOOLS::logspace` | Returns `num` logarithmically spaced reals between `start` and `stop` (optional base). | E3 `src/TOOLS.f90:19`; `src/tools_grids.f90:32-47` |
| XP-064 | library API `TOOLS::arange` | Returns `num` consecutive integers starting at `start`. | E3 `src/TOOLS.f90:20`; `src/tools_grids.f90:51-63` |
| XP-065 | library API `TOOLS::powspace` | Returns `num` points from `start` to `stop` with geometric (`base`) spacing. | E3 `src/TOOLS.f90:21`; `src/tools_grids.f90:147-156` |
| XP-066 | library API `TOOLS::upmspace` | Builds a uniform-power mesh between two endpoints. | E3 `src/TOOLS.f90:22`; `src/tools_grids.f90:96-144` |
| XP-067 | library API `TOOLS::upminterval` | Builds a two-sided uniform-power mesh through a midpoint. | E3 `src/TOOLS.f90:23`; `src/tools_grids.f90:68-80` |
| XP-068 | library API `TOOLS::sort` | In-place insertion sort of a real array of length `M`. | E3 `src/TOOLS.f90:26`; `src/tools_sort1d.f90:70-84` |
| XP-069 | library API `TOOLS::sort_array` | Sorts an array and optionally returns the permutation of labels. | E3 `src/TOOLS.f90:26`; `src/tools_sort1d.f90:92-94` |
| XP-070 | library API `TOOLS::uniq` | Compacts an integer or real array to unique entries (optional mask), using `unista`. | E3 `src/TOOLS.f90:27`; `src/tools_sort1d.f90:8-38` |
| XP-071 | library API `TOOLS::reshuffle` | Permutes a real array according to an integer `order`. | E3 `src/TOOLS.f90:27`; `src/tools_sort1d.f90:51-59` |
| XP-072 | library API `TOOLS::shiftFW` | Copies a 0-based matrix or array into a 1-based result, shifted forward by `step`. | E3 `src/TOOLS.f90:28,49-51`; `src/tools_shifts.f90:6-15` |
| XP-073 | library API `TOOLS::shiftBW` | Copies a 1-based matrix or array into a 0-based result, shifted backward by `step`. | E3 `src/TOOLS.f90:28,53-55`; `src/tools_shifts.f90:70` |
| XP-074 | library API `TOOLS::deriv` | Centered finite-difference derivative of a real array with spacing `dh` (one-sided at ends). | E3 `src/TOOLS.f90:31,175-186` |
| XP-075 | library API `TOOLS::gfbethe` | Returns the Hilbert transform of `zeta` with a Bethe DOS of half-bandwidth `d`. | E3 `src/TOOLS.f90:34`; `src/tools_bethe.f90:63-75` |
| XP-076 | library API `TOOLS::gfbether` | Elemental Bethe-DOS Hilbert transform variant taking frequency `w`, `zeta`, and `d`. | E3 `src/TOOLS.f90:35`; `src/tools_bethe.f90:84-86` |
| XP-077 | library API `TOOLS::bethe_lattice` | Fills DOS and energy arrays for a Bethe lattice and writes `LATTICEinfo/DOSbethe.lattice`. | E3 `src/TOOLS.f90:36`; `src/tools_bethe.f90:5-28` |
| XP-078 | library API `TOOLS::dens_bethe` | Non-interacting Bethe-lattice density of states at energy `x`. | E3 `src/TOOLS.f90:37`; `src/tools_bethe.f90:41-50` |
| XP-079 | library API `TOOLS::dens_hyperc` | Non-interacting hypercubic-lattice DOS (Gaussian) at energy `x`. | E3 `src/TOOLS.f90:38,193-202` |
| XP-080 | library API `TOOLS::find2Dmesh` | Returns nearest-neighbor indices of a 2-d point on `gridX`/`gridY`. | E3 `src/TOOLS.f90:41,328-338` |
| XP-081 | library API `TOOLS::get_matsubara_gf_from_dos` | Builds a Matsubara Green's function by integrating a spectral density. | E3 `src/TOOLS.f90:44,285-312` |
| XP-082 | library API `TOOLS::check_convergence` | Tests convergence of successive 0-d/1-d/2-d integer, real, or complex arrays and may write error files. | E3 `src/TOOLS.f90:45,88-99` |
| XP-083 | library API `TOOLS::check_convergence_scalar` | Tests convergence of successive scalar 0-d/1-d/2-d integer, real, or complex values. | E3 `src/TOOLS.f90:45,75-86` |
| XP-084 | library API `TOOLS::check_convergence_local` | Local (per-index) convergence test of successive 0-d/1-d/2-d arrays. | E3 `src/TOOLS.f90:45,101-112` |
| XP-085 | library API `TOOLS::get_free_dos` | Writes a non-interacting DOS file and optionally moves it under `LATTICEinfo/`. | E3 `src/TOOLS.f90:46,244-276` |
| XP-086 | library API `TOOLS::sum_overk_zeta` | Sums `wk/(zeta-ek)` over k-points (equal weights if `wk` omitted). | E3 `src/TOOLS.f90:47,231-238` |
| XP-087 | library API `TOOLS::uniinv` | Merge-sort inverse ranking of a real or integer array with duplicates removed. | E3 `src/TOOLS.f90:57-60,417-649` |
| XP-088 | library API `TOOLS::unista` | Stable unique: removes duplicates, leaving first appearances (optional mask). `nearless` has an interface in this module but is not `public ::`. | E3 `src/TOOLS.f90:62-69,1181-1198` |
| XP-089 | library API `BROYDEN::broydn` | Broyden solver for a nonlinear system `funcv(x)=0` (Numerical Recipes-style; optional `maxits`/`tolf`/`tolmin`/`stpmx`/`noexit`). | E3 `src/BROYDEN.f90:6,29` |
| XP-090 | library API `BRENT::fzero` | Finds a real root of `func` on `[a,b]` by calling `zbrent` with `epsilon(a)` tolerance. | E3 `src/BRENT.f90:5,11-24` |
| XP-091 | library API `BRENT::zbrent` | Brent root finder for `func` on `[x1,x2]` with given `tol` (max 100 iterations). | E3 `src/BRENT.f90:6,29-40` |
| XP-092 | library API `WRAP_MINPACK::fsolve` | Calls MINPACK `hybrd1` in place on `x` (optional `tol`/`info`). | E3 `src/WRAP_MINPACK.f90:5,9-22` |
| XP-093 | library API `WRAP_MINPACK::ffsolve` | Same `hybrd1` solve as `fsolve`, returning the updated `x` as a function result. | E3 `src/WRAP_MINPACK.f90:5,24-39` |
| XP-094 | library API `FFTGF::cfft_1d_forward` | In-place radix-2 forward complex FFT via `four1` (aborts if length is not a power of two). Compiled unit is symlink `src/FFTGF.f90` → `FFTGF_NR.f90`. | E3 `src/FFTGF.f90:6,28-32` |
| XP-095 | library API `FFTGF::cfft_1d_backward` | In-place radix-2 backward complex FFT via `four1`. | E3 `src/FFTGF.f90:6,34-38` |
| XP-096 | library API `FFTGF::cfft_1d_shift` | Rearranges a length-`2*L` FFT array onto `-L:L` ordering. | E3 `src/FFTGF.f90:6,40-47` |
| XP-097 | library API `FFTGF::swap_fftrt2rw` | Swaps the two halves of a complex FFT array (rt↔rw layout). | E3 `src/FFTGF.f90:6,49-59` |
| XP-098 | library API `FFTGF::fftgf_rw2rt` | FFT of a Green's function from real frequency (length `2*M`) to real time (`-M:M`). | E3 `src/FFTGF.f90:7,75-80` |
| XP-099 | library API `FFTGF::fftgf_rt2rw` | FFT of a Green's function from real time (`-M:M`) to real frequency (length `2*M`). | E3 `src/FFTGF.f90:7,104-117` |
| XP-100 | library API `FFTGF::fftgf_iw2tau` | FFT from Matsubara frequencies to imaginary time on `[0,beta]`, with tail subtraction and reshaping. | E3 `src/FFTGF.f90:8,138-149` |
| XP-101 | library API `FFTGF::fftgf_tau2iw` | FFT from imaginary time to Matsubara frequencies, interpolating onto a finer mesh first. | E3 `src/FFTGF.f90:8,247-265` |
| XP-102 | library API `FFTGF::fftff_iw2tau` | Cosine-transform path from Matsubara frequencies to imaginary time (complex `gw`). | E3 `src/FFTGF.f90:9,210-235` |
| XP-103 | library API `FFTGF::fftff_tau2iw` | FFT from complex imaginary-time data to Matsubara frequencies. | E3 `src/FFTGF.f90:9,267-289` |
| XP-104 | library API `FFTGF::fftff_iw2tau_` | Direct cosine sum from Matsubara `gw` to imaginary time (requires `L==N`). | E3 `src/FFTGF.f90:10,200-208` |
| XP-105 | library API `FFTGF::four1` | Numerical Recipes mixed-radix complex FFT kernel used by the NR backend. | E3 `src/FFTGF.f90:11,426-483` |
| XP-106 | library API `FFTGF::cosft2` | Numerical Recipes cosine transform of a real array. | E3 `src/FFTGF.f90:11,306-370` |
| XP-107 | library API `FFTGF::realft` | Numerical Recipes real-valued FFT helper used by `cosft2`. | E3 `src/FFTGF.f90:11,372` |
| XP-108 | library API `FUNCTIONS::heaviside` | Elemental Heaviside: `0` for `x<0`, `0.5` at `x==0`, `1` for `x>0`. | E3 `src/FUNCTIONS.f90:179,198-208` |
| XP-109 | library API `FUNCTIONS::step` | Step function: `1` for `x>=0` by default, or `1` only for `x>0` when `origin=.false.`. | E3 `src/FUNCTIONS.f90:180,214-227` |
| XP-110 | library API `FUNCTIONS::fermi` | Fermi-Dirac function `1/(1+exp(beta*x))` with overflow guards at `|beta*x|>100`. | E3 `src/FUNCTIONS.f90:181,240-248` |
| XP-111 | library API `FUNCTIONS::sgn` | Sign of an integer or real argument. | E3 `src/FUNCTIONS.f90:182-185` |
| XP-112 | library API `FUNCTIONS::wfun` | Complex Faddeeva / scaled complementary error function (includes `functions_wofz.f90`). | E3 `src/FUNCTIONS.f90:188,284-294` |
| XP-113 | library API `FUNCTIONS::zerf` | Error-function evaluation (includes `functions_zerf.f90`). | E3 `src/FUNCTIONS.f90:189,302` |
| XP-114 | library API `GREENFUNX::allocate_gf` | Allocates pointer components of `matsubara_gf`, `real_gf`, or `keldysh_equilibrium_gf` types. | E3 `src/GREENFUNX.f90:37-41,63` |
| XP-115 | library API `GREENFUNX::deallocate_gf` | Deallocates those Green's-function type pointer components. | E3 `src/GREENFUNX.f90:43-47,64` |
| XP-116 | library API `GREENFUNX::assignment(=)` | Assigns Green's-function types from another instance or from a scalar. | E3 `src/GREENFUNX.f90:49-56,65` |
| XP-117 | library API `GREENFUNX::operator(+)` | Adds matching Green's-function types componentwise. | E3 `src/GREENFUNX.f90:58-66,182-187` |
| XP-118 | library API `GREENFUNX::ret_component_t` | Retarded component in time: `step(t)*(gtr-less)`. | E3 `src/GREENFUNX.f90:67,78-96` |
| XP-119 | library API `GREENFUNX::less_component_w` | Lesser component in frequency from a retarded function, `wr`, and `beta`. | E3 `src/GREENFUNX.f90:68,101-122` |
| XP-120 | library API `GREENFUNX::gtr_component_w` | Greater component in frequency from a retarded function, `wr`, and `beta`. | E3 `src/GREENFUNX.f90:69,127-148` |
| XP-121 | library API `INTEGRATE::kramers_kronig` | Kramers–Kronig integral using QUADPACK `QAWCE` and order-5 local interpolation. | E3 `src/INTEGRATE.f90:62,320-353` |
| XP-122 | library API `INTEGRATE::kronig` | Faster Kramers–Kronig integral using a logarithmic kernel and finite-difference derivative. | E3 `src/INTEGRATE.f90:63,282-309` |
| XP-123 | library API `INTEGRATE::trapz` | Trapezoidal integration of real or complex data (interval, spacing, or nonlinear abscissa). | E3 `src/INTEGRATE.f90:47-52,64,76-88` |
| XP-124 | library API `INTEGRATE::simps` | Simpson integration of real or complex data (interval, spacing, or nonlinear abscissa). | E3 `src/INTEGRATE.f90:54-59,65,161` |
| XP-125 | library API `INTEGRATE::init_finter` | Allocates a `d_finter` or `c_finter` interpolant object of given size. | E3 `src/INTEGRATE.f90:31-33,67,381-394` |
| XP-126 | library API `INTEGRATE::kill_finter` | Deallocates a `d_finter` or `c_finter` interpolant object. | E3 `src/INTEGRATE.f90:35-37,68,397-412` |
| XP-127 | library API `INTEGRATE::set_finter` | Fills interpolant X/F arrays, index range, and polynomial order. | E3 `src/INTEGRATE.f90:39-41,69,415-424` |
| XP-128 | library API `INTEGRATE::finter_func` | Polynomial interpolation of a `d_finter` or `c_finter` object at abscissa `x`. | E3 `src/INTEGRATE.f90:43-45,70,438-456` |
| XP-129 | library API `SQUARE_LATTICE::square_lattice_dispersion` | 2-d square-lattice dispersion `-2t(cos kx + cos ky) - 4t' cos kx cos ky`. Not re-exported by `SCIFOR`. | E3 `src/SQUARE_LATTICE.f90:37,56-68` |
| XP-130 | library API `SQUARE_LATTICE::square_lattice_velocity` | Group velocity on the 2-d square lattice for hoppings `ts`/`tsp`. Not re-exported by `SCIFOR`. | E3 `src/SQUARE_LATTICE.f90:38,84-96` |
| XP-131 | library API `SQUARE_LATTICE::square_lattice_dimension` | Returns the number of k-points `Lk` for an `Nx` (optional `Ny`) grid. Not re-exported by `SCIFOR`. | E3 `src/SQUARE_LATTICE.f90:39,111-117` |
| XP-132 | library API `SQUARE_LATTICE::square_lattice_structure` | Builds the 2-d square-lattice k-grid, index maps, and weights. Not re-exported by `SCIFOR`. | E3 `src/SQUARE_LATTICE.f90:40,129-131` |
| XP-133 | library API `SQUARE_LATTICE::square_lattice_dispersion_array` | Fills `epsik(ik)` from the stored k-grid and hoppings. Not re-exported by `SCIFOR`. | E3 `src/SQUARE_LATTICE.f90:41,270-290` |
| XP-134 | library API `SQUARE_LATTICE::square_lattice_MGXMpath_dimension` | Returns the length of the M–Γ–X–M path for `Nx`/`Ny`. Not re-exported by `SCIFOR`. | E3 `src/SQUARE_LATTICE.f90:42,305-308` |
| XP-135 | library API `SQUARE_LATTICE::square_lattice_MGXMpath_structure` | Fills M–Γ–X–M path k-points, dispersion, and high-symmetry indices. Not re-exported by `SCIFOR`. | E3 `src/SQUARE_LATTICE.f90:43,324-362` |
| XP-136 | library API `SQUARE_LATTICE::square_lattice_reduxGrid_dimension` | Counts a stride-reduced k-grid length. Not re-exported by `SCIFOR`. | E3 `src/SQUARE_LATTICE.f90:44,376-391` |
| XP-137 | library API `SQUARE_LATTICE::square_lattice_reduxGrid_index` | Fills the index map of a stride-reduced k-grid. Not re-exported by `SCIFOR`. | E3 `src/SQUARE_LATTICE.f90:45,408` |
| XP-138 | library API `SQUARE_LATTICE::square_lattice_reduxGrid_dispersion_array` | Gathers `epsik` onto the reduced-grid index list. Not re-exported by `SCIFOR`. | E3 `src/SQUARE_LATTICE.f90:46,436-444` |
| XP-139 | library API `VECTORS::operator(+)` | Adds `vect2D`/`vect3D` values. Compiled via `include` in `SQUARE_LATTICE.f90` (not a Makefile target); not re-exported by `SCIFOR`. | E3 `src/VECTORS.f90:24-26,54`; `src/SQUARE_LATTICE.f90:7-9` |
| XP-140 | library API `VECTORS::operator(-)` | Subtracts `vect2D`/`vect3D` values. Compiled via `SQUARE_LATTICE` include; not re-exported by `SCIFOR`. | E3 `src/VECTORS.f90:27-29,55`; `src/SQUARE_LATTICE.f90:7-9` |
| XP-141 | library API `VECTORS::operator(*)` | Left/right scalar multiply of `vect2D`/`vect3D`. Compiled via `SQUARE_LATTICE` include; not re-exported by `SCIFOR`. | E3 `src/VECTORS.f90:30-32,56`; `src/SQUARE_LATTICE.f90:7-9` |
| XP-142 | library API `VECTORS::operator(.dot.)` | Dot product of `vect2D`/`vect3D`. Compiled via `SQUARE_LATTICE` include; not re-exported by `SCIFOR`. | E3 `src/VECTORS.f90:33-35,57`; `src/SQUARE_LATTICE.f90:7-9` |
| XP-143 | library API `VECTORS::assignment(=)` | Assigns `vect2D`/`vect3D` from a vector or scalar. Compiled via `SQUARE_LATTICE` include; not re-exported by `SCIFOR`. | E3 `src/VECTORS.f90:36-38,58`; `src/SQUARE_LATTICE.f90:7-9` |
| XP-144 | library API `VECTORS::modulo` | Euclidean modulus of `vect2D`/`vect3D`. Compiled via `SQUARE_LATTICE` include; not re-exported by `SCIFOR`. | E3 `src/VECTORS.f90:39-41,59`; `src/SQUARE_LATTICE.f90:7-9` |
| XP-145 | library API `D_ORDERED_LIST::init_list` | Allocates an empty ordered real linked list. Not re-exported by `SCIFOR`. | E3 `src/LIST_D_ORDERED.f90:45,61-66` |
| XP-146 | library API `D_ORDERED_LIST::destroy_list` | Walks and deallocates an ordered list. Not re-exported by `SCIFOR`. | E3 `src/LIST_D_ORDERED.f90:46,69-86` |
| XP-147 | library API `D_ORDERED_LIST::insert_element` | Generic alias of `insert_element_before`. Not re-exported by `SCIFOR`. | E3 `src/LIST_D_ORDERED.f90:41-47` |
| XP-148 | library API `D_ORDERED_LIST::insert_element_before` | Inserts a `node_object` into the ordered list before the first node `>= obj`. Not re-exported by `SCIFOR`. | E3 `src/LIST_D_ORDERED.f90:47,89-108` |
| XP-149 | library API `D_ORDERED_LIST::insert_element_after` | Inserts a `node_object` after the matching ordered position. Not re-exported by `SCIFOR`. | E3 `src/LIST_D_ORDERED.f90:47` |
| XP-150 | library API `D_ORDERED_LIST::remove_element` | Removes a node matching `obj` from the ordered list. Not re-exported by `SCIFOR`. | E3 `src/LIST_D_ORDERED.f90:48` |
| XP-151 | library API `D_ORDERED_LIST::get_value` | Returns the `node_object` stored in a list node. Not re-exported by `SCIFOR`. | E3 `src/LIST_D_ORDERED.f90:49,166-170` |
| XP-152 | library API `D_ORDERED_LIST::get_node` | Finds the node whose value matches `value`. Not re-exported by `SCIFOR`. | E3 `src/LIST_D_ORDERED.f90:50,173` |
| XP-153 | library API `D_ORDERED_LIST::print_list` | Prints list node values to stdout. Not re-exported by `SCIFOR`. | E3 `src/LIST_D_ORDERED.f90:51,192-205` |
| XP-154 | library API `D_ORDERED_LIST::dump_list` | Copies list values into a real vector. Not re-exported by `SCIFOR`. | E3 `src/LIST_D_ORDERED.f90:51,209-226` |
| XP-155 | library API `D_UNORDERED_LIST::init_list` | Allocates an empty unordered real linked list (`size=0`). Not re-exported by `SCIFOR`. | E3 `src/LIST_D_UNORDERED.f90:17,27-33` |
| XP-156 | library API `D_UNORDERED_LIST::destroy_list` | Walks and deallocates an unordered real list. Not re-exported by `SCIFOR`. | E3 `src/LIST_D_UNORDERED.f90:18,36` |
| XP-157 | library API `D_UNORDERED_LIST::add_element` | Appends a real value to the unordered list. Not re-exported by `SCIFOR`. | E3 `src/LIST_D_UNORDERED.f90:19` |
| XP-158 | library API `D_UNORDERED_LIST::remove_element` | Removes a real value from the unordered list. Not re-exported by `SCIFOR`. | E3 `src/LIST_D_UNORDERED.f90:20` |
| XP-159 | library API `D_UNORDERED_LIST::get_value` | Returns the real stored in a list node. Not re-exported by `SCIFOR`. | E3 `src/LIST_D_UNORDERED.f90:21` |
| XP-160 | library API `D_UNORDERED_LIST::get_node` | Finds the node holding a given real. Not re-exported by `SCIFOR`. | E3 `src/LIST_D_UNORDERED.f90:22` |
| XP-161 | library API `D_UNORDERED_LIST::print_list` | Prints unordered real list values to stdout. Not re-exported by `SCIFOR`. | E3 `src/LIST_D_UNORDERED.f90:23` |
| XP-162 | library API `D_UNORDERED_LIST::dump_list` | Copies unordered real list values into a vector. Not re-exported by `SCIFOR`. | E3 `src/LIST_D_UNORDERED.f90:23` |
| XP-163 | library API `Z_UNORDERED_LIST::init_list` | Allocates an empty unordered complex linked list. Not re-exported by `SCIFOR`. | E3 `src/LIST_Z_UNORDERED.f90:17,27-33` |
| XP-164 | library API `Z_UNORDERED_LIST::destroy_list` | Walks and deallocates an unordered complex list. Not re-exported by `SCIFOR`. | E3 `src/LIST_Z_UNORDERED.f90:18` |
| XP-165 | library API `Z_UNORDERED_LIST::add_element` | Appends a complex value to the unordered list. Not re-exported by `SCIFOR`. | E3 `src/LIST_Z_UNORDERED.f90:19` |
| XP-166 | library API `Z_UNORDERED_LIST::remove_element` | Removes a complex value from the unordered list. Not re-exported by `SCIFOR`. | E3 `src/LIST_Z_UNORDERED.f90:20` |
| XP-167 | library API `Z_UNORDERED_LIST::get_value` | Returns the complex stored in a list node. Not re-exported by `SCIFOR`. | E3 `src/LIST_Z_UNORDERED.f90:21` |
| XP-168 | library API `Z_UNORDERED_LIST::get_node` | Finds the node holding a given complex. Not re-exported by `SCIFOR`. | E3 `src/LIST_Z_UNORDERED.f90:22` |
| XP-169 | library API `Z_UNORDERED_LIST::print_list` | Prints unordered complex list values to stdout. Not re-exported by `SCIFOR`. | E3 `src/LIST_Z_UNORDERED.f90:23` |
| XP-170 | library API `Z_UNORDERED_LIST::dump_list` | Copies unordered complex list values into a vector. Not re-exported by `SCIFOR`. | E3 `src/LIST_Z_UNORDERED.f90:23` |
| XP-171 | library API `MATRIX::matrix_diagonalize` | LAPACK symmetric/Hermitian eigen-decomposition (`dsyev`/`zheev`). | E3 `src/MATRIX.f90:46-48,63,86-105` |
| XP-172 | library API `MATRIX::solve_linear_system` | LAPACK solution of `A*x=b` for one or many right-hand sides (real or complex). | E3 `src/MATRIX.f90:52-55,65,146` |
| XP-173 | library API `MATRIX::matrix_inverse` | LAPACK inverse of a general real or complex matrix. | E3 `src/MATRIX.f90:14-16,67,307` |
| XP-174 | library API `MATRIX::matrix_inverse_sym` | LAPACK inverse of a symmetric real or complex matrix. | E3 `src/MATRIX.f90:18-20,68,428` |
| XP-175 | library API `MATRIX::matrix_inverse_her` | LAPACK inverse of a Hermitian complex matrix. | E3 `src/MATRIX.f90:22-24,69,496` |
| XP-176 | library API `MATRIX::matrix_inverse_triang` | LAPACK inverse of a triangular real or complex matrix. | E3 `src/MATRIX.f90:26-28,70,372` |
| XP-177 | library API `MATRIX::matrix_inverse_gj` | Gauss–Jordan inverse of a general real or complex matrix. | E3 `src/MATRIX.f90:30-32,71,555` |
| XP-178 | library API `MATRIX::m_invert` | Function-style interface to general matrix inversion. | E3 `src/MATRIX.f90:40-42,73,84` |
| XP-179 | library API `MATRIX::m_invert_gj` | Function-style interface to Gauss–Jordan inversion. | E3 `src/MATRIX.f90:36-38,74,265` |
| XP-180 | library API `SPLINE::poly_spline` | Polynomial (Neville) interpolation of real or complex data. | E3 `src/SPLINE.f90:18-20,34,91` |
| XP-181 | library API `SPLINE::cubic_spline` | Cubic-spline interpolation of real or complex data. | E3 `src/SPLINE.f90:22-24,35` |
| XP-182 | library API `SPLINE::linear_spline` | Linear interpolation of real or complex data onto new abscissas. | E3 `src/SPLINE.f90:26-28,36,47-50` |
| XP-183 | library API `SPLINE::extract` | Public alias of `extract_gtau`. | E3 `src/SPLINE.f90:30-37` |
| XP-184 | library API `SPLINE::extract_gtau` | Samples `G(tau)` from `N+1` points down to `Nfak+1` points on `[0,beta]`. | E3 `src/SPLINE.f90:37,262-269` |
| XP-185 | library API `SPLINE::interp_gtau` | Interpolates `G(tau)` from one tau mesh onto another via `CUBSPL`/`PPVALU`. | E3 `src/SPLINE.f90:38,230-246` |
| XP-186 | library API `SPLINE_FINTER_MOD::finter` | Polynomial interpolation using module-level `finterX`/`finterF` tables. Compiled via `include` in `SPLINE.f90`; not re-exported by `SPLINE` (`private`) or `SCIFOR`. | E3 `src/spline_finter_mod.f90:9,13-32`; `src/SPLINE.f90:2,14-16` |
| XP-187 | library API `RANDOM::nrand` | Park–Miller-style uniform `(0,1)` generator with integer seed `dseed`. | E3 `src/RANDOM.f90:26,145-177` |
| XP-188 | library API `RANDOM::irand` | Integer `nint(random_number*10)` via the Fortran intrinsic RNG. | E3 `src/RANDOM.f90:27,34-39` |
| XP-189 | library API `RANDOM::drand` | Uniform real in `(0,1)` via `random_number`. | E3 `src/RANDOM.f90:27,40-43` |
| XP-190 | library API `RANDOM::crand` | Uniform complex with independent real and imaginary `random_number` draws. | E3 `src/RANDOM.f90:27,44-50` |
| XP-191 | library API `RANDOM::init_random_number` | Seeds the intrinsic RNG from `SYSTEM_CLOCK` (optional integer shift). | E3 `src/RANDOM.f90:28,197-208` |
| XP-192 | library API `RANDOM::random_order` | Fills `order(1:n)` with a random permutation of `1…n`. | E3 `src/RANDOM.f90:29,254-276` |
| XP-193 | library API `RANDOM::rand` | Fills a scalar, vector, or matrix of integer, real, or complex values with `drand()*dim`. | E3 `src/RANDOM.f90:20-24,30,63-68` |
| XP-194 | library API `STATISTICS::histogram_allocate` | Allocates a `histogram` with `n` bins and `n+1` range edges. | E3 `src/STATISTICS.f90:11,121-131` |
| XP-195 | library API `STATISTICS::histogram_set_range_uniform` | Sets uniform bin edges on `[xmin,xmax]` and zeros counts. | E3 `src/STATISTICS.f90:12,134-150` |
| XP-196 | library API `STATISTICS::histogram_accumulate` | Adds weight `w` to the bin containing `x`. | E3 `src/STATISTICS.f90:13,153-164` |
| XP-197 | library API `STATISTICS::histogram_get_range` | Returns `[lower,upper]` edges of bin `index`. | E3 `src/STATISTICS.f90:14,195-205` |
| XP-198 | library API `STATISTICS::histogram_get_value` | Returns the accumulated weight of bin `index`. | E3 `src/STATISTICS.f90:15,208-217` |
| XP-199 | library API `STATISTICS::histogram_print` | Writes bin ranges and values to a Fortran unit. | E3 `src/STATISTICS.f90:16,220-235` |
| XP-200 | library API `STATISTICS::get_moments` | Computes mean, standard deviation, variance, skewness, and kurtosis of a real vector. | E3 `src/STATISTICS.f90:19,27-39` |
| XP-201 | library API `STATISTICS::get_mean` | Returns the mean via `get_moments`. | E3 `src/STATISTICS.f90:20` |
| XP-202 | library API `STATISTICS::get_sd` | Returns the standard deviation via `get_moments`. | E3 `src/STATISTICS.f90:20` |
| XP-203 | library API `STATISTICS::get_var` | Returns the variance via `get_moments`. | E3 `src/STATISTICS.f90:20,80-84` |
| XP-204 | library API `STATISTICS::get_skew` | Returns the skewness via `get_moments`. | E3 `src/STATISTICS.f90:20,87-91` |
| XP-205 | library API `STATISTICS::get_curt` | Returns the kurtosis via `get_moments`. | E3 `src/STATISTICS.f90:20,94-98` |
| XP-206 | library API `STATISTICS::get_covariance` | Sample covariance matrix of rows of `data` given a `mean` vector. | E3 `src/STATISTICS.f90:21,101-117` |
| XP-207 | library API `PADE::pade_analytic_continuation` | Padé continuation of Matsubara `gm(wm)` onto points `x`. Not re-exported by `SCIFOR`. | E3 `src/PADE.f90:9,14-27` |
| XP-208 | CLI `numutils/src/deriv.f90` (`program deriv_`) | Reads `X,Y` (or `Y` with `dh=`) from stdin and prints a numerical derivative. Wired in `numutils` `all`. | E3 `numutils/src/deriv.f90:1,18-32`; `numutils/src/Makefile:8` |
| XP-209 | CLI `numutils/src/kdensity.f90` (`program kdensity`) | Reads `x(:)` from stdin and prints a Gaussian kernel-density estimate (Silverman's rule). Wired in `all`. | E3 `numutils/src/kdensity.f90:1,25-40`; `numutils/src/Makefile:8` |
| XP-210 | CLI `numutils/src/numstat.f90` (`program numstat`) | Reads columns from stdin and prints selected moments / min / max / covariance. Wired in `all`. | E3 `numutils/src/numstat.f90:1,34-39`; `numutils/src/Makefile:8` |
| XP-211 | CLI `numutils/src/splot.f90` (`program plot_3d`) | Reads `Z(:)` from stdin and writes a 3-d gnuplot data file plus script. Wired in `all`. | E3 `numutils/src/splot.f90:1,23-34`; `numutils/src/Makefile:8` |
| XP-212 | CLI `numutils/src/func.f90` (`program func`) | Reads `x(:)` from stdin and evaluates a `libmatheval` expression `f(x)` (help text: experimental). Wired in `all`. | E3 `numutils/src/func.f90:1,19-34`; `numutils/src/Makefile:8,46` |
| XP-213 | CLI `numutils/src/wmatsubara.f90` (`program linsp`) | Prints `L` fermionic Matsubara frequencies `π(2n-1)/beta`. Wired in `all`. | E3 `numutils/src/wmatsubara.f90:1,10-23`; `numutils/src/Makefile:8` |
| XP-214 | CLI `numutils/src/fftgf.f90` (`program fftgf_`) | Reads complex arrays and applies `FFTGF` transforms (`fw`/`bw`/`rt2rw`/`rw2rt`/`iw2tau`/`tau2iw`). Wired in `all`. | E3 `numutils/src/fftgf.f90:1,23-38`; `numutils/src/Makefile:8` |
| XP-215 | CLI `numutils/src/arange.f90` (`program arange_`) | Prints `L` reals `1…L`. Wired in `all`. | E3 `numutils/src/arange.f90:1,9-21`; `numutils/src/Makefile:8` |
| XP-216 | CLI `numutils/src/pade.f90` (`program pade_`) | Padé-continues a Matsubara Green's function from `fin` onto a real-axis grid. Wired in `all`. | E3 `numutils/src/pade.f90:1,23-38`; `numutils/src/Makefile:8` |
| XP-217 | CLI `numutils/src/logspace.f90` (`program logsp`) | Prints `L` logarithmically spaced reals on `[wmin,wmax]`. Wired in `all`. | E3 `numutils/src/logspace.f90:1,11-27`; `numutils/src/Makefile:8` |
| XP-218 | CLI `numutils/src/linspace.f90` (`program linsp`) | Prints `L` evenly spaced reals on `[wmin,wmax]`. Wired in `all`. | E3 `numutils/src/linspace.f90:1,11-26`; `numutils/src/Makefile:8` |
| XP-219 | CLI `numutils/src/fermi.f90` (`program fermi_`) | Reads `A(:)` from stdin and prints the Fermi function at `beta`. Wired in `all`. | E3 `numutils/src/fermi.f90:1,17-30`; `numutils/src/Makefile:8` |
| XP-220 | CLI `numutils/src/spline.f90` (`program spline_`) | Reads `Y` or `X,Y` from stdin and prints cubic, linear, or polynomial interpolation on a finer mesh. Wired in `all`. | E3 `numutils/src/spline.f90:1,18-34`; `numutils/src/Makefile:8` |
| XP-221 | CLI `numutils/src/random.f90` (`program random_`) | Prints `N` random integer, real, or complex samples on `[min,max]`. Wired in `all`. | E3 `numutils/src/random.f90:1,14-30`; `numutils/src/Makefile:8` |
| XP-222 | CLI `numutils/src/histogram.f90` (`program histogram_`) | Reads `X(:)` from stdin and prints a binned histogram. Wired in `all`. | E3 `numutils/src/histogram.f90:1,21-35`; `numutils/src/Makefile:8` |
| XP-223 | CLI `scripts/build.sh` | Experiment clone: sources `sciforvars.sh`, symlinks an FFT backend (default `NR`), runs `src/Makefile`, and links `fidelity/driver.f90` to `.bin/scifor-fidelity`. | E3 `scripts/build.sh:1-99` |
| XP-224 | CLI `scripts/fidelity.sh` | Runs `.bin/scifor-fidelity`, extracts numbered sections, and compares them to `fidelity/golden/` within `TOL`. | E3 `scripts/fidelity.sh:1-11,161-184` |
| XP-225 | CLI `fidelity/driver.f90` (`program scifor_fidelity`) | Prints `linspace`/`logspace`/`arange`/`fermi`/`deriv` sections used by `scripts/fidelity.sh`. | E3 `fidelity/driver.f90:1-67` |

## 2. Open inventory questions

- [ ] Which, if any, consumers call the ~165 file-scope special-function subroutines compiled into `FUNCTIONS.o` via `EXTERNAL` rather than `USE FUNCTIONS`?
- [ ] Do any consumers `USE SQUARE_LATTICE`, `D_ORDERED_LIST`/`D_UNORDERED_LIST`/`Z_UNORDERED_LIST`, `PADE`, `VECTORS`, or `SPLINE_FINTER_MOD` after linking, or do they only `USE SCIFOR`?
- [ ] Which FFT backend is the intended default? `src/FFTGF.f90` currently symlinks to `FFTGF_NR.f90`; `scripts/build.sh` defaults `FFT_BACKEND=NR` but can select `FFTW3` or `MKL`; `bin/setup_sf.sh` tried to pick MKL/FFTW3/NR using filenames that are not in this tree (`FFT_MKL.f90`, `FFTGF.90`).
- [ ] Is `bin/setup_sf.sh` still the installer, or has `scripts/build.sh` replaced it in this experiment clone?
- [ ] Can `numutils` targets `vfplot`, `ffcmplx`, or `diagPAM` still be triggered? `vfplot` `USE`s `DLPLOT` (not in `allmod`); `diagPAM.f90` is absent from the tree.
- [ ] Are standalone modules `m_uniinv` (`src/uniinv.f90`) and `M_UNISTA` (`src/unista.f90`) ever `USE`d, or is the inlined copy inside `TOOLS` the only live entry?

## Appendix. Inactive, unknown, and retired paths

| ID | Trigger | Status | Why classified this way | Evidence |
|----|---------|--------|-------------------------|----------|
| XP-226 | file-scope special-function pack `src/functions_special_funcs.f90` (~165 `subroutine`/`function` names) | unknown | `INCLUDE`d *before* `MODULE FUNCTIONS` in `src/FUNCTIONS.f90`, so the pack compiles into `FUNCTIONS.o` but is not on `MODULE FUNCTIONS`'s `public ::` list. Cannot tell whether any caller reaches them via `EXTERNAL`. | E5 `src/FUNCTIONS.f90:171-189`; ~165 file-scope procedures in `src/functions_special_funcs.f90` |
| XP-227 | character-pack `src/CHRPACK.f90` (~315 file-scope routines) | suspected-dead | Source present; not a `src/Makefile` `allmod` target and not `INCLUDE`d by any compiled module. | E3 `src/Makefile:6`; `src/CHRPACK.f90` exists and is not referenced by `allmod` |
| XP-228 | library API `m_uniinv::uniinv` (`src/uniinv.f90`) | suspected-dead | Standalone module with `public :: uniinv` is not a Makefile target; `TOOLS` already inlines the same ranking code. | E3 `src/uniinv.f90:10-22`; absent from `src/Makefile:6` |
| XP-229 | library API `M_UNISTA::unista` (`src/unista.f90`) | suspected-dead | Standalone module with `public :: unista` is not a Makefile target; `TOOLS` already inlines `unista`. | E3 `src/unista.f90:4-13`; absent from `src/Makefile:6` |
| XP-230 | file-scope RNG pack `src/random_routines.f90` | suspected-dead | `RANDOM` include of this file is commented out; not a Makefile target. | E3 `src/RANDOM.f90:291`; absent from `src/Makefile:6` |
| XP-231 | library API `FFTGF::cfft_1d_ex` | unknown | Extra `public ::` name on unused MKL and FFTW3 backends only; current compile unit `src/FFTGF.f90` → `FFTGF_NR.f90` does not export it. `scripts/build.sh` can symlink those backends. | E3 `src/FFTGF_MKL.f90:11`; `src/FFTGF_FFTW3.f90:5`; `src/FFTGF.f90:6`; `scripts/build.sh:50-62` |
| XP-232 | library API module `FFTGF_FFTPACK` (`src/FFTGF_FFTPACK.f90`) | suspected-dead | Differently named module; not compiled by `src/Makefile` and not a `scripts/build.sh` `FFT_BACKEND` choice (`NR`/`FFTW3`/`MKL` only). | E3 `src/FFTGF_FFTPACK.f90:1-6`; `src/Makefile:106-108`; `scripts/build.sh:50-62` |
| XP-233 | CLI `bin/setup_sf.sh` | suspected-dead | Installer assumes `$SFROOT/sf` layout, sources missing `bin/mylibvars.sh`, and `ln`s missing `FFT_MKL.f90` / `FFT_FFTW3.f90` / `FFT_NR.f90` onto `FFTGF.90`. | E3 `bin/setup_sf.sh:7-12,196-205`; `bin/mylibvars.sh` and `src/FFT_MKL.f90` absent |
| XP-234 | CLI `numutils/src/vfplot.f90` (`program vfplot`) | unknown | Make target exists but is not in `all`; `USE DLPLOT` (no `DLPLOT` in `src/Makefile` `allmod`) and `USE VECTORS`. | E3 `numutils/src/Makefile:8,84-86`; `numutils/src/vfplot.f90:1-7` |
| XP-235 | CLI `numutils/src/ffcmplx.f90` (`program ffcmplx`) | unknown | Make target exists but is not in `all`; no other in-tree reference found. | E3 `numutils/src/Makefile:8,24-26`; `numutils/src/ffcmplx.f90:1,15-32` |
| XP-236 | CLI `numutils/src/Makefile` target `diagPAM` | unknown | Make target compiles `dfftpack.o` plus `diagPAM.f90`, but `diagPAM.f90` is not in the tree and the target is not in `all`. | E3 `numutils/src/Makefile:8,20-22`; `diagPAM.f90` absent |
| XP-237 | library/module `STRING_PACK` | retired | Not present in this tree. Commit message states it was moved to `.repo` and “never really used”. | E4 git `2915e6b` (“STRING_PACK has been moved to .repo (never really used)”) |

## Errata

None.

## Links

- Dependency graph: `docs/modernization/dependency-graph.md`
- Assessment: `docs/modernization/ASSESSMENT.md`
