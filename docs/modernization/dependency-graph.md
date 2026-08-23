# Dependency Graph

**Legacy repo:** `/Users/philipteitel/code/ADD-migrations/sci-fortran-legacy` (`e586903a26cc50ca8942f20ca3bccbd8814e6252`)
**Version:** `v1`
**Status:** `Snapshot`
**Date:** `2026-08-22`
**Owner:** Archaeologist (`/map-dependency-graph`)

## Summary

This graph joins all **237** execution paths from the path inventory (225 active + 12 appendix) to internal Fortran modules/procedures and the **29** `DEP-NNN` IDs in dependency-inventory.md v1 Draft. Edges follow implementation, compilation-unit includes, and the trigger that runs the path — not the full `libscifor.a` link line on every public symbol. Relative to that inventory (dispositions cited, not copied as graph state), paths whose recovered externals are disjoint from `no-route` **DEP-015** and `undecided` **DEP-002, DEP-003, DEP-012, DEP-014, DEP-018, DEP-020, DEP-024, DEP-028** are XP-003–XP-088, XP-092, XP-093, XP-108–XP-120, XP-122–XP-127, XP-129–XP-170, XP-181–XP-185, XP-187–XP-207, XP-224, XP-228–XP-230, XP-237 (180 IDs). Widely shared externals are **DEP-001** (every compiled Fortran unit/CLI that is actually built), then **DEP-003**/**DEP-004** on consumer `numutils`/install/build triggers, **DEP-029** on `mpiID`-gated diagnostics and timers, **DEP-014** on the current NR `FFTGF` cluster plus `scripts/build.sh`, and **DEP-009**/**DEP-010**/**DEP-012** on LAPACK `MATRIX` paths. Structurally, oracle.md already executed **XP-062** `linspace`, **XP-063** `logspace`, **XP-110** `fermi`, and **XP-074** `deriv` via **XP-223**/**XP-224**/**XP-225**; XP-225's arange-labeled section prints `real(i,8)` and does not call `TOOLS::arange`. Sequencing belongs in the migration plan, not this file. Next: `/analyze-impedance`.

---

## 1. Path to internals and externals

| XP ID | Internal modules / functions | External DEP IDs | Evidence |
|-------|------------------------------|------------------|----------|
| XP-001 | `src/Makefile` `all`/`version`/`allmod`; `$(FC)` compile of `allmod` units; `ar`/`ranlib`/`rsync`/`mv` of `libscifor.a`/`libscifor_deb.a` and `.mod`; includes `options.inc` | DEP-001, DEP-002, DEP-003, DEP-004, DEP-005, DEP-006, DEP-028 | E3 `src/Makefile:1-22,24-25`; `include/options.inc:1-16` |
| XP-002 | `bin/sciforvars.sh` exports `SFLIB`/`SFINCLUDE`/`SFETC`/`SFBIN` and loader paths; `source $SFETC/library.conf` | DEP-002, DEP-003, DEP-007 | E3 `bin/sciforvars.sh:1-38`; `etc/library.conf:1-9` (`export FC=ifort`) |
| XP-003 | `COMMON_VARS::version`; `include scifor_version.inc`; calls `timestamp`; gates `mpiID==0` | DEP-001, DEP-006, DEP-029 | E3 `src/COMVARS.f90:9,61,92,112-124`; `src/Makefile:2,24-25` |
| XP-004 | `COMMON_VARS::timestamp`; `date_and_time`; gates `mpiID==0` | DEP-001, DEP-029 | E3 `src/COMVARS.f90:61,93,136-145` |
| XP-005 | `COMMON_VARS::abort` interface to `error` | DEP-001, DEP-029 | E3 `src/COMVARS.f90:86-88,94,192-209` |
| XP-006 | `COMMON_VARS::error`; gates `mpiID`/`mpiSIZE`; `stop` | DEP-001, DEP-029 | E3 `src/COMVARS.f90:61,94,192-209` |
| XP-007 | `COMMON_VARS::warning`; gates `mpiID`/`mpiSIZE` | DEP-001, DEP-029 | E3 `src/COMVARS.f90:61,94,211-227` |
| XP-008 | `COMMON_VARS::msg`; gates `mpiID`/`mpiSIZE` | DEP-001, DEP-029 | E3 `src/COMVARS.f90:61,95,235-254` |
| XP-009 | `COMMON_VARS::txtfy` (`i_to_ch`/`r_to_ch`/`c_to_ch`) | DEP-001 | E3 `src/COMVARS.f90:82-84,96,261-284` |
| XP-010 | `COMMON_VARS::bold` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:97,290-294` |
| XP-011 | `COMMON_VARS::underline` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:98,296-300` |
| XP-012 | `COMMON_VARS::highlight` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:99,302-306` |
| XP-013 | `COMMON_VARS::erased` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:100,308-312` |
| XP-014 | `COMMON_VARS::red` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:101,314-318` |
| XP-015 | `COMMON_VARS::green` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:101,320-324` |
| XP-016 | `COMMON_VARS::yellow` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:101,326-330` |
| XP-017 | `COMMON_VARS::blue` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:101,332-336` |
| XP-018 | `COMMON_VARS::purple` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:101,338-342` |
| XP-019 | `COMMON_VARS::cyan` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:101,344-348` |
| XP-020 | `COMMON_VARS::bold_red` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:102,350-354` |
| XP-021 | `COMMON_VARS::bold_green` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:102,356-360` |
| XP-022 | `COMMON_VARS::bold_yellow` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:102,362-366` |
| XP-023 | `COMMON_VARS::bold_blue` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:102,368-372` |
| XP-024 | `COMMON_VARS::bold_purple` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:102,374-378` |
| XP-025 | `COMMON_VARS::bold_cyan` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:102,380-384` |
| XP-026 | `COMMON_VARS::bg_red` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:103,386-390` |
| XP-027 | `COMMON_VARS::bg_green` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:103,392-396` |
| XP-028 | `COMMON_VARS::bg_yellow` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:103,398-402` |
| XP-029 | `COMMON_VARS::bg_blue` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:103,404-408` |
| XP-030 | `COMMON_VARS::bg_purple` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:103,410-414` |
| XP-031 | `COMMON_VARS::bg_cyan` ANSI SGR wrapper | DEP-001 | E3 `src/COMVARS.f90:103,416-420` |
| XP-032 | `PARSE_CMD::parse_cmd_variable`; `USE COMMON_VARS, only: msg`; `command_argument_count`/`get_command_argument` | DEP-001 | E3 `src/PARSECMD.f90:6,18-23,109-128` |
| XP-033 | `PARSE_CMD::get_cmd_variable` | DEP-001 | E3 `src/PARSECMD.f90:24,87-100` |
| XP-034 | `PARSE_CMD::parse_cmd_help` | DEP-001 | E3 `src/PARSECMD.f90:25,36-61` |
| XP-035 | `PARSE_CMD::print_cmd_help` | DEP-001 | E3 `src/PARSECMD.f90:26,70-78` |
| XP-036 | `TIMER::print_bar`; include `timer_bar.f90`; gates `mpiID==0` | DEP-001, DEP-029 | E3 `src/TIMER.f90:8,35,41-43`; `src/timer_bar.f90:6-25` |
| XP-037 | `TIMER::start_timer`; include `timer_chrono.f90`; `date_and_time`; gates `mpiID==0` | DEP-001, DEP-029 | E3 `src/TIMER.f90:36,41`; `src/timer_chrono.f90:5-18` |
| XP-038 | `TIMER::stop_timer`; gates `mpiID==0` | DEP-001, DEP-029 | E3 `src/TIMER.f90:36`; `src/timer_chrono.f90:31-45` |
| XP-039 | `TIMER::eta`; gates `mpiID==0` | DEP-001, DEP-029 | E3 `src/TIMER.f90:37`; `src/timer_chrono.f90:126-157` |
| XP-040 | `IOFILE::file_size`; `fstat`; `USE COMMON_VARS` | DEP-001 | E3 `src/IOFILE.f90:8,31,102-118` |
| XP-041 | `IOFILE::file_length`; may call `data_open` (`gunzip`) for `.gz` sibling | DEP-001, DEP-027 | E3 `src/IOFILE.f90:32,174-199,249-268` |
| XP-042 | `IOFILE::file_info`; `fstat` | DEP-001 | E3 `src/IOFILE.f90:33,131-161` |
| XP-043 | `IOFILE::data_open`; `SYSTEM("gunzip …")` | DEP-001, DEP-027 | E3 `src/IOFILE.f90:34,249-268` |
| XP-044 | `IOFILE::data_store`; `SYSTEM("gzip -fv …")` | DEP-001, DEP-027 | E3 `src/IOFILE.f90:35,219-232` |
| XP-045 | `IOFILE::set_store_size` | DEP-001 | E3 `src/IOFILE.f90:36,234-238` |
| XP-046 | `IOFILE::reg_filename` | DEP-001 | E3 `src/IOFILE.f90:37,49-53` |
| XP-047 | `IOFILE::reg` alias of `reg_filename` | DEP-001 | E3 `src/IOFILE.f90:15-17,37` |
| XP-048 | `IOFILE::txtfit` alias of `reg_filename` | DEP-001 | E3 `src/IOFILE.f90:19-21,37` |
| XP-049 | `IOFILE::txtcut` alias of `reg_filename` | DEP-001 | E3 `src/IOFILE.f90:23-25,37` |
| XP-050 | `IOFILE::create_data_dir`; `SYSTEM("mkdir -v …")`; gates `mpiID` | DEP-001, DEP-027, DEP-029 | E3 `src/IOFILE.f90:38,281-292` |
| XP-051 | `IOFILE::create_dir` alias of `create_data_dir` | DEP-001, DEP-027, DEP-029 | E3 `src/IOFILE.f90:27-29,38,281-292` |
| XP-052 | `IOFILE::close_file` | DEP-001 | E3 `src/IOFILE.f90:39,77-82` |
| XP-053 | `IOFILE::get_filename` | DEP-001 | E3 `src/IOFILE.f90:40,55-64` |
| XP-054 | `IOFILE::get_filepath` | DEP-001 | E3 `src/IOFILE.f90:41,66-75` |
| XP-055 | `SLPLOT::splot`; includes `slplot_splot_P/V/M.f90`; `USE IOFILE`, `USE COMMON_VARS` | DEP-001 | E3 `src/SLPLOT.f90:7-8,12-25,41,49-55` |
| XP-056 | `SLPLOT::splot3d`; include `slplot_splot_3d.f90`; writes `gnuplot -persist` `.gp` scripts; `SYSTEM("chmod +x …")` | DEP-001, DEP-025, DEP-027 | E3 `src/SLPLOT.f90:28-31,42,57-58`; `src/slplot_splot_3d.f90:38-66` |
| XP-057 | `SLPLOT::store_data`; include `slplot_data_save.f90` | DEP-001 | E3 `src/SLPLOT.f90:34-43,61` |
| XP-058 | `SLREAD::sread`; includes `slread_sread_P/V/M.f90`; `USE IOFILE` | DEP-001 | E3 `src/SLREAD.f90:7,13-29,41,46-52` |
| XP-059 | `SLREAD::read_data`; include `slread_data_read.f90` | DEP-001 | E3 `src/SLREAD.f90:31-42,55` |
| XP-060 | `TOOLS::start_loop`; `USE COMMON_VARS`/`TIMER`; calls `start_timer`; gates `mpiID` | DEP-001, DEP-029 | E3 `src/TOOLS.f90:7-9,14,121-140` |
| XP-061 | `TOOLS::end_loop`; calls `stop_timer`; gates `mpiID` | DEP-001, DEP-029 | E3 `src/TOOLS.f90:15,147-158` |
| XP-062 | `TOOLS::linspace`; include `tools_grids.f90` | DEP-001 | E1 `src/TOOLS.f90:18`; `src/tools_grids.f90:1-29`; oracle `fidelity/driver.f90:10-16` |
| XP-063 | `TOOLS::logspace`; include `tools_grids.f90` | DEP-001 | E1 `src/TOOLS.f90:19`; `src/tools_grids.f90:32-47`; oracle `fidelity/driver.f90:19-25` |
| XP-064 | `TOOLS::arange`; include `tools_grids.f90` | DEP-001 | E3 `src/TOOLS.f90:20`; `src/tools_grids.f90:51-63` |
| XP-065 | `TOOLS::powspace`; include `tools_grids.f90` | DEP-001 | E3 `src/TOOLS.f90:21`; `src/tools_grids.f90:147-156` |
| XP-066 | `TOOLS::upmspace`; include `tools_grids.f90` | DEP-001 | E3 `src/TOOLS.f90:22`; `src/tools_grids.f90:96-144` |
| XP-067 | `TOOLS::upminterval`; include `tools_grids.f90` | DEP-001 | E3 `src/TOOLS.f90:23`; `src/tools_grids.f90:68-80` |
| XP-068 | `TOOLS::sort`; include `tools_sort1d.f90` | DEP-001 | E3 `src/TOOLS.f90:26`; `src/tools_sort1d.f90:70-84` |
| XP-069 | `TOOLS::sort_array`; include `tools_sort1d.f90` | DEP-001 | E3 `src/TOOLS.f90:26`; `src/tools_sort1d.f90:92-94` |
| XP-070 | `TOOLS::uniq`; calls `unista`; include `tools_sort1d.f90` | DEP-001 | E3 `src/TOOLS.f90:27,62-70`; `src/tools_sort1d.f90:8-38` |
| XP-071 | `TOOLS::reshuffle`; include `tools_sort1d.f90` | DEP-001 | E3 `src/TOOLS.f90:27`; `src/tools_sort1d.f90:51-59` |
| XP-072 | `TOOLS::shiftFW`; include `tools_shifts.f90` | DEP-001 | E3 `src/TOOLS.f90:28,49-51`; `src/tools_shifts.f90:6-15` |
| XP-073 | `TOOLS::shiftBW`; include `tools_shifts.f90` | DEP-001 | E3 `src/TOOLS.f90:28,53-55`; `src/tools_shifts.f90:70` |
| XP-074 | `TOOLS::deriv` centered finite difference | DEP-001 | E1 `src/TOOLS.f90:31,175-186`; oracle `fidelity/driver.f90:41-66` |
| XP-075 | `TOOLS::gfbethe`; include `tools_bethe.f90` | DEP-001 | E3 `src/TOOLS.f90:34`; `src/tools_bethe.f90:63-75` |
| XP-076 | `TOOLS::gfbether`; include `tools_bethe.f90` | DEP-001 | E3 `src/TOOLS.f90:35`; `src/tools_bethe.f90:84-86` |
| XP-077 | `TOOLS::bethe_lattice`; `SYSTEM` `mkdir LATTICEinfo`; writes `DOSbethe.lattice` | DEP-001, DEP-027 | E3 `src/TOOLS.f90:36`; `src/tools_bethe.f90:5-28` |
| XP-078 | `TOOLS::dens_bethe`; include `tools_bethe.f90` | DEP-001 | E3 `src/TOOLS.f90:37`; `src/tools_bethe.f90:41-50` |
| XP-079 | `TOOLS::dens_hyperc` | DEP-001 | E3 `src/TOOLS.f90:38,193-202` |
| XP-080 | `TOOLS::find2Dmesh` | DEP-001 | E3 `src/TOOLS.f90:41,328-338` |
| XP-081 | `TOOLS::get_matsubara_gf_from_dos` | DEP-001 | E3 `src/TOOLS.f90:44,285-312` |
| XP-082 | `TOOLS::check_convergence`; includes `tools_check_function1d.f90` | DEP-001 | E3 `src/TOOLS.f90:45,88-99,214` |
| XP-083 | `TOOLS::check_convergence_scalar`; include `tools_check_scalar.f90` | DEP-001 | E3 `src/TOOLS.f90:45,75-86,214` |
| XP-084 | `TOOLS::check_convergence_local`; include `tools_check_function1d_local.f90` | DEP-001 | E3 `src/TOOLS.f90:45,101-112,216` |
| XP-085 | `TOOLS::get_free_dos`; `SYSTEM` `mkdir`/`mv` into `LATTICEinfo/` | DEP-001, DEP-027 | E3 `src/TOOLS.f90:46,244-276` |
| XP-086 | `TOOLS::sum_overk_zeta` | DEP-001 | E3 `src/TOOLS.f90:47,231-238` |
| XP-087 | `TOOLS::uniinv` (inlined in `TOOLS`) | DEP-001 | E3 `src/TOOLS.f90:57-60,417-649` |
| XP-088 | `TOOLS::unista` (inlined in `TOOLS`) | DEP-001 | E3 `src/TOOLS.f90:62-69,1181-1198` |
| XP-089 | `BROYDEN::broydn`; include `broydn_routines.f90`; `USE BROYDN_ROUTINES` | DEP-001, DEP-020 | E3 `src/BROYDEN.f90:1-6,29`; `src/broydn_routines.f90` |
| XP-090 | `BRENT::fzero` calls `zbrent` | DEP-001, DEP-020 | E3 `src/BRENT.f90:5,11-24,29-40` |
| XP-091 | `BRENT::zbrent` | DEP-001, DEP-020 | E3 `src/BRENT.f90:6,29-40` |
| XP-092 | `WRAP_MINPACK::fsolve`; include `minpack.f90`; `hybrd1` | DEP-001, DEP-017 | E3 `src/WRAP_MINPACK.f90:1-5,9-22`; `src/minpack.f90:1383` |
| XP-093 | `WRAP_MINPACK::ffsolve`; `hybrd1` | DEP-001, DEP-017 | E3 `src/WRAP_MINPACK.f90:5,24-39`; `src/minpack.f90:1383` |
| XP-094 | `FFTGF::cfft_1d_forward`; current unit `src/FFTGF.f90` → `FFTGF_NR.f90`; `four1`; `USE TOOLS`, `USE SPLINE` | DEP-001, DEP-014 | E3 `src/FFTGF.f90:1-6,28-32`; `src/FFTGF_NR.f90:28-32,426-483` |
| XP-095 | `FFTGF::cfft_1d_backward`; `four1` | DEP-001, DEP-014 | E3 `src/FFTGF.f90:6,34-38`; `src/FFTGF_NR.f90:34-38` |
| XP-096 | `FFTGF::cfft_1d_shift` (NR compile unit) | DEP-001, DEP-014 | E3 `src/FFTGF.f90:6,40-47` |
| XP-097 | `FFTGF::swap_fftrt2rw` (NR compile unit) | DEP-001, DEP-014 | E3 `src/FFTGF.f90:6,49-59` |
| XP-098 | `FFTGF::fftgf_rw2rt`; calls `cfft_1d_forward`/`cfft_1d_shift` | DEP-001, DEP-014 | E3 `src/FFTGF.f90:7,75-80` |
| XP-099 | `FFTGF::fftgf_rt2rw`; calls `cfft_1d_backward` | DEP-001, DEP-014 | E3 `src/FFTGF.f90:7,104-117` |
| XP-100 | `FFTGF::fftgf_iw2tau`; `cfft_1d_backward`/`cfft_1d_shift` or `fftgf_rw2rt` | DEP-001, DEP-014 | E3 `src/FFTGF.f90:8,138-189` |
| XP-101 | `FFTGF::fftgf_tau2iw`; include `splinefft.f90` (`CUBSPL`/`PPVALU`); `fftgf_rt2rw` | DEP-001, DEP-014, DEP-021 | E3 `src/FFTGF.f90:8,247-265`; `src/splinefft.f90:11-19` |
| XP-102 | `FFTGF::fftff_iw2tau`; `cosft2`; `TOOLS::linspace`; `SPLINE::linear_spline` | DEP-001, DEP-014 | E3 `src/FFTGF.f90:9,210-235,306-370` |
| XP-103 | `FFTGF::fftff_tau2iw`; include `splinefft.f90`; `fftgf_rt2rw` | DEP-001, DEP-014, DEP-021 | E3 `src/FFTGF.f90:9,267-289`; `src/splinefft.f90:11-19` |
| XP-104 | `FFTGF::fftff_iw2tau_` direct cosine sum (NR compile unit) | DEP-001, DEP-014 | E3 `src/FFTGF.f90:10,192-208` |
| XP-105 | `FFTGF::four1` NR mixed-radix kernel | DEP-001, DEP-014 | E3 `src/FFTGF.f90:11,426-483` |
| XP-106 | `FFTGF::cosft2` NR cosine transform | DEP-001, DEP-014 | E3 `src/FFTGF.f90:11,306-370` |
| XP-107 | `FFTGF::realft` NR real FFT helper | DEP-001, DEP-014 | E3 `src/FFTGF.f90:11,372` |
| XP-108 | `FUNCTIONS::heaviside`; `USE COMMON_VARS` (does not call Zhang/Jin pack) | DEP-001 | E3 `src/FUNCTIONS.f90:173-179,198-208` |
| XP-109 | `FUNCTIONS::step` | DEP-001 | E3 `src/FUNCTIONS.f90:180,214-227` |
| XP-110 | `FUNCTIONS::fermi` | DEP-001 | E1 `src/FUNCTIONS.f90:181,240-248`; oracle `fidelity/driver.f90:32-38` |
| XP-111 | `FUNCTIONS::sgn` (`i_sgn`/`d_sgn`) | DEP-001 | E3 `src/FUNCTIONS.f90:182-185` |
| XP-112 | `FUNCTIONS::wfun`; contained include `functions_wofz.f90`; `wofz` | DEP-001, DEP-022 | E3 `src/FUNCTIONS.f90:188,284-294` |
| XP-113 | `FUNCTIONS::zerf`; include `functions_zerf.f90` | DEP-001, DEP-022 | E3 `src/FUNCTIONS.f90:189,301-302` |
| XP-114 | `GREENFUNX::allocate_gf`; include `greenfunx_allocate_gf.f90`; `USE COMMON_VARS` | DEP-001 | E3 `src/GREENFUNX.f90:9,37-41,63,158` |
| XP-115 | `GREENFUNX::deallocate_gf`; include `greenfunx_deallocate_gf.f90` | DEP-001 | E3 `src/GREENFUNX.f90:43-47,64,159` |
| XP-116 | `GREENFUNX::assignment(=)`; includes `greenfunx_gf_identity.f90`, `greenfunx_scalar_identity.f90` | DEP-001 | E3 `src/GREENFUNX.f90:49-56,65,170-171` |
| XP-117 | `GREENFUNX::operator(+)` componentwise add | DEP-001 | E3 `src/GREENFUNX.f90:58-66,182-211` |
| XP-118 | `GREENFUNX::ret_component_t`; contained local `step` (not `FUNCTIONS::step`) | DEP-001 | E3 `src/GREENFUNX.f90:67,78-96` |
| XP-119 | `GREENFUNX::less_component_w`; contained local `fermi` | DEP-001 | E3 `src/GREENFUNX.f90:68,101-122` |
| XP-120 | `GREENFUNX::gtr_component_w`; contained local `fermi` | DEP-001 | E3 `src/GREENFUNX.f90:69,127-148` |
| XP-121 | `INTEGRATE::kramers_kronig`; `QAWCE`; local `polint`/`locate` via `kkfinter`; `src/Makefile` also compiles `integrate_d_quadpack.f90` | DEP-001, DEP-016, DEP-020 | E3 `src/INTEGRATE.f90:62,320-371,479-537`; `src/Makefile:149-151`; `src/integrate_d_quadpack.f90:2502` |
| XP-122 | `INTEGRATE::kronig` (local finite-difference `deriv` array; no QUADPACK) | DEP-001 | E3 `src/INTEGRATE.f90:63,282-312` |
| XP-123 | `INTEGRATE::trapz` (no QUADPACK/`polint`) | DEP-001 | E3 `src/INTEGRATE.f90:47-52,64,76-88` |
| XP-124 | `INTEGRATE::simps` (no QUADPACK/`polint`) | DEP-001 | E3 `src/INTEGRATE.f90:54-59,65,161` |
| XP-125 | `INTEGRATE::init_finter` | DEP-001 | E3 `src/INTEGRATE.f90:31-33,67,381-394` |
| XP-126 | `INTEGRATE::kill_finter` | DEP-001 | E3 `src/INTEGRATE.f90:35-37,68,397-412` |
| XP-127 | `INTEGRATE::set_finter` | DEP-001 | E3 `src/INTEGRATE.f90:39-41,69,415-424` |
| XP-128 | `INTEGRATE::finter_func`; local `polint`/`locate` | DEP-001, DEP-020 | E3 `src/INTEGRATE.f90:43-45,70,438-476,479-537` |
| XP-129 | `SQUARE_LATTICE::square_lattice_dispersion`; `USE VECTORS`, `USE COMMON_VARS` | DEP-001 | E3 `src/SQUARE_LATTICE.f90:7-11,37,56-68` |
| XP-130 | `SQUARE_LATTICE::square_lattice_velocity` | DEP-001 | E3 `src/SQUARE_LATTICE.f90:38,84-96` |
| XP-131 | `SQUARE_LATTICE::square_lattice_dimension` | DEP-001 | E3 `src/SQUARE_LATTICE.f90:39,111-117` |
| XP-132 | `SQUARE_LATTICE::square_lattice_structure`; `SYSTEM` `mkdir`/`mv` `*.lattice` | DEP-001, DEP-027 | E3 `src/SQUARE_LATTICE.f90:40,129-160` |
| XP-133 | `SQUARE_LATTICE::square_lattice_dispersion_array` | DEP-001 | E3 `src/SQUARE_LATTICE.f90:41,270-290` |
| XP-134 | `SQUARE_LATTICE::square_lattice_MGXMpath_dimension` | DEP-001 | E3 `src/SQUARE_LATTICE.f90:42,305-308` |
| XP-135 | `SQUARE_LATTICE::square_lattice_MGXMpath_structure` | DEP-001 | E3 `src/SQUARE_LATTICE.f90:43,324-362` |
| XP-136 | `SQUARE_LATTICE::square_lattice_reduxGrid_dimension` | DEP-001 | E3 `src/SQUARE_LATTICE.f90:44,376-391` |
| XP-137 | `SQUARE_LATTICE::square_lattice_reduxGrid_index` | DEP-001 | E3 `src/SQUARE_LATTICE.f90:45,408` |
| XP-138 | `SQUARE_LATTICE::square_lattice_reduxGrid_dispersion_array` | DEP-001 | E3 `src/SQUARE_LATTICE.f90:46,436-444` |
| XP-139 | `VECTORS::operator(+)`; compiled via `include VECTORS.f90` in `SQUARE_LATTICE.f90` | DEP-001 | E3 `src/VECTORS.f90:24-26,54`; `src/SQUARE_LATTICE.f90:7-9` |
| XP-140 | `VECTORS::operator(-)` | DEP-001 | E3 `src/VECTORS.f90:27-29,55`; `src/SQUARE_LATTICE.f90:7-9` |
| XP-141 | `VECTORS::operator(*)` | DEP-001 | E3 `src/VECTORS.f90:30-32,56`; `src/SQUARE_LATTICE.f90:7-9` |
| XP-142 | `VECTORS::operator(.dot.)` | DEP-001 | E3 `src/VECTORS.f90:33-35,57`; `src/SQUARE_LATTICE.f90:7-9` |
| XP-143 | `VECTORS::assignment(=)` | DEP-001 | E3 `src/VECTORS.f90:36-38,58`; `src/SQUARE_LATTICE.f90:7-9` |
| XP-144 | `VECTORS::modulo` | DEP-001 | E3 `src/VECTORS.f90:39-41,59`; `src/SQUARE_LATTICE.f90:7-9` |
| XP-145 | `D_ORDERED_LIST::init_list` | DEP-001 | E3 `src/LIST_D_ORDERED.f90:45,61-66` |
| XP-146 | `D_ORDERED_LIST::destroy_list` | DEP-001 | E3 `src/LIST_D_ORDERED.f90:46,69-86` |
| XP-147 | `D_ORDERED_LIST::insert_element` alias of `insert_element_before` | DEP-001 | E3 `src/LIST_D_ORDERED.f90:41-47` |
| XP-148 | `D_ORDERED_LIST::insert_element_before` | DEP-001 | E3 `src/LIST_D_ORDERED.f90:47,89-111` |
| XP-149 | `D_ORDERED_LIST::insert_element_after` | DEP-001 | E3 `src/LIST_D_ORDERED.f90:47,113-135` |
| XP-150 | `D_ORDERED_LIST::remove_element` | DEP-001 | E3 `src/LIST_D_ORDERED.f90:48` |
| XP-151 | `D_ORDERED_LIST::get_value` | DEP-001 | E3 `src/LIST_D_ORDERED.f90:49,166-170` |
| XP-152 | `D_ORDERED_LIST::get_node` | DEP-001 | E3 `src/LIST_D_ORDERED.f90:50,173` |
| XP-153 | `D_ORDERED_LIST::print_list` | DEP-001 | E3 `src/LIST_D_ORDERED.f90:51,192-205` |
| XP-154 | `D_ORDERED_LIST::dump_list` | DEP-001 | E3 `src/LIST_D_ORDERED.f90:51,209-226` |
| XP-155 | `D_UNORDERED_LIST::init_list` | DEP-001 | E3 `src/LIST_D_UNORDERED.f90:17,27-33` |
| XP-156 | `D_UNORDERED_LIST::destroy_list` | DEP-001 | E3 `src/LIST_D_UNORDERED.f90:18,36` |
| XP-157 | `D_UNORDERED_LIST::add_element` | DEP-001 | E3 `src/LIST_D_UNORDERED.f90:19,56-79` |
| XP-158 | `D_UNORDERED_LIST::remove_element` | DEP-001 | E3 `src/LIST_D_UNORDERED.f90:20,82-99` |
| XP-159 | `D_UNORDERED_LIST::get_value` | DEP-001 | E3 `src/LIST_D_UNORDERED.f90:21,102-106` |
| XP-160 | `D_UNORDERED_LIST::get_node` | DEP-001 | E3 `src/LIST_D_UNORDERED.f90:22,109-128` |
| XP-161 | `D_UNORDERED_LIST::print_list` | DEP-001 | E3 `src/LIST_D_UNORDERED.f90:23,131-144` |
| XP-162 | `D_UNORDERED_LIST::dump_list` | DEP-001 | E3 `src/LIST_D_UNORDERED.f90:23,148-163` |
| XP-163 | `Z_UNORDERED_LIST::init_list` | DEP-001 | E3 `src/LIST_Z_UNORDERED.f90:17,27-33` |
| XP-164 | `Z_UNORDERED_LIST::destroy_list` | DEP-001 | E3 `src/LIST_Z_UNORDERED.f90:18` |
| XP-165 | `Z_UNORDERED_LIST::add_element` | DEP-001 | E3 `src/LIST_Z_UNORDERED.f90:19,56-79` |
| XP-166 | `Z_UNORDERED_LIST::remove_element` | DEP-001 | E3 `src/LIST_Z_UNORDERED.f90:20,82-99` |
| XP-167 | `Z_UNORDERED_LIST::get_value` | DEP-001 | E3 `src/LIST_Z_UNORDERED.f90:21,102-106` |
| XP-168 | `Z_UNORDERED_LIST::get_node` | DEP-001 | E3 `src/LIST_Z_UNORDERED.f90:22,109-128` |
| XP-169 | `Z_UNORDERED_LIST::print_list` | DEP-001 | E3 `src/LIST_Z_UNORDERED.f90:23,131-144` |
| XP-170 | `Z_UNORDERED_LIST::dump_list` | DEP-001 | E3 `src/LIST_Z_UNORDERED.f90:23,148-163` |
| XP-171 | `MATRIX::matrix_diagonalize`; `dsyev`/`zheev`; `include mkl_lapack.fi`; `USE COMMON_VARS` | DEP-001, DEP-009, DEP-010, DEP-012 | E3 `src/MATRIX.f90:7-10,46-48,63,86-105,123-129` |
| XP-172 | `MATRIX::solve_linear_system`; `dgetrf`/`dgetrs`/`zgetrf`/`zgetrs` | DEP-001, DEP-009, DEP-010, DEP-012 | E3 `src/MATRIX.f90:10,52-55,65,163-245` |
| XP-173 | `MATRIX::matrix_inverse`; `dgetrf`/`dgetri`/`zgetrf`/`zgetri` | DEP-001, DEP-009, DEP-010, DEP-012 | E3 `src/MATRIX.f90:10,14-16,67,307-348` |
| XP-174 | `MATRIX::matrix_inverse_sym`; `dsytrf`/`dsytri`/`zsytrf`/`zsytri` | DEP-001, DEP-009, DEP-010, DEP-012 | E3 `src/MATRIX.f90:10,18-20,68,442-473` |
| XP-175 | `MATRIX::matrix_inverse_her`; `zhetrf`/`zhetri` | DEP-001, DEP-009, DEP-010, DEP-012 | E3 `src/MATRIX.f90:10,22-24,69,524-530` |
| XP-176 | `MATRIX::matrix_inverse_triang`; `dtrtri`/`ztrtri` | DEP-001, DEP-009, DEP-010, DEP-012 | E3 `src/MATRIX.f90:10,26-28,70,386-404` |
| XP-177 | `MATRIX::matrix_inverse_gj` Gauss–Jordan; `swap` interface commented `from nr90`; no LAPACK calls | DEP-001, DEP-020 | E3 `src/MATRIX.f90:30-32,58-61,71,555-605` |
| XP-178 | `MATRIX::m_invert` calls `matrix_inverse` | DEP-001, DEP-009, DEP-010, DEP-012 | E3 `src/MATRIX.f90:40-42,73,280-291` |
| XP-179 | `MATRIX::m_invert_gj` calls `matrix_inverse_GJ` | DEP-001, DEP-020 | E3 `src/MATRIX.f90:36-38,74,267-278` |
| XP-180 | `SPLINE::poly_spline` (`d_fspline`); `USE SPLINE_FINTER_MOD`; `finter` → `polint` | DEP-001, DEP-020 | E3 `src/SPLINE.f90:1-2,13-14,18-20,34,94-124`; `src/spline_finter_mod.f90:13-32`; `src/spline_nr_mod.f90:11,42` |
| XP-181 | `SPLINE::cubic_spline`; `CUBSPL`/`PPVALU`; include `spline_cubspl_routines.f90` | DEP-001, DEP-021 | E3 `src/SPLINE.f90:4,22-24,35,156-181` |
| XP-182 | `SPLINE::linear_spline` (native linear; no cubspl/`polint`) | DEP-001 | E3 `src/SPLINE.f90:26-28,36,47-80` |
| XP-183 | `SPLINE::extract` alias of `extract_gtau` | DEP-001 | E3 `src/SPLINE.f90:30-37,262-279` |
| XP-184 | `SPLINE::extract_gtau` subsample (no cubspl/`polint`) | DEP-001 | E3 `src/SPLINE.f90:37,262-279` |
| XP-185 | `SPLINE::interp_gtau`; `CUBSPL`/`PPVALU` | DEP-001, DEP-021 | E3 `src/SPLINE.f90:38,230-246` |
| XP-186 | `SPLINE_FINTER_MOD::finter`; `USE SPLINE_NR_MOD`; `locate`/`polint`; included from `SPLINE.f90` | DEP-001, DEP-020 | E3 `src/spline_finter_mod.f90:1-32`; `src/SPLINE.f90:2,14-16` |
| XP-187 | `RANDOM::nrand` Park–Miller; `random_routines.f90` include is commented | DEP-001 | E3 `src/RANDOM.f90:26,145-177,291` |
| XP-188 | `RANDOM::irand`; Fortran `random_number` | DEP-001 | E3 `src/RANDOM.f90:27,34-39` |
| XP-189 | `RANDOM::drand`; Fortran `random_number` | DEP-001 | E3 `src/RANDOM.f90:27,40-43` |
| XP-190 | `RANDOM::crand`; Fortran `random_number` | DEP-001 | E3 `src/RANDOM.f90:27,44-50` |
| XP-191 | `RANDOM::init_random_number`; `SYSTEM_CLOCK`/`random_seed` | DEP-001 | E3 `src/RANDOM.f90:28,197-208` |
| XP-192 | `RANDOM::random_order` | DEP-001 | E3 `src/RANDOM.f90:29,254-276` |
| XP-193 | `RANDOM::rand` fills via `drand` | DEP-001 | E3 `src/RANDOM.f90:20-24,30,63-68` |
| XP-194 | `STATISTICS::histogram_allocate` | DEP-001 | E3 `src/STATISTICS.f90:11,121-131` |
| XP-195 | `STATISTICS::histogram_set_range_uniform` | DEP-001 | E3 `src/STATISTICS.f90:12,134-150` |
| XP-196 | `STATISTICS::histogram_accumulate` | DEP-001 | E3 `src/STATISTICS.f90:13,153-164` |
| XP-197 | `STATISTICS::histogram_get_range` | DEP-001 | E3 `src/STATISTICS.f90:14,195-205` |
| XP-198 | `STATISTICS::histogram_get_value` | DEP-001 | E3 `src/STATISTICS.f90:15,208-217` |
| XP-199 | `STATISTICS::histogram_print` | DEP-001 | E3 `src/STATISTICS.f90:16,220-235` |
| XP-200 | `STATISTICS::get_moments` | DEP-001 | E3 `src/STATISTICS.f90:19,27-63` |
| XP-201 | `STATISTICS::get_mean` via `get_moments` | DEP-001 | E3 `src/STATISTICS.f90:20,66-70` |
| XP-202 | `STATISTICS::get_sd` via `get_moments` | DEP-001 | E3 `src/STATISTICS.f90:20,73-77` |
| XP-203 | `STATISTICS::get_var` via `get_moments` | DEP-001 | E3 `src/STATISTICS.f90:20,80-84` |
| XP-204 | `STATISTICS::get_skew` via `get_moments` | DEP-001 | E3 `src/STATISTICS.f90:20,87-91` |
| XP-205 | `STATISTICS::get_curt` via `get_moments` | DEP-001 | E3 `src/STATISTICS.f90:20,94-98` |
| XP-206 | `STATISTICS::get_covariance` | DEP-001 | E3 `src/STATISTICS.f90:21,101-117` |
| XP-207 | `PADE::pade_analytic_continuation`; `USE COMMON_VARS`, `USE TOOLS` | DEP-001 | E3 `src/PADE.f90:2-4,9,14-27` |
| XP-208 | `program deriv_`; `TOOLS::deriv`; `D_UNORDERED_LIST`; `PARSE_CMD`; `numutils/src/Makefile` `${FC}` `${SFLIBS}` | DEP-001, DEP-003, DEP-004 | E3 `numutils/src/deriv.f90:1-6,77`; `numutils/src/Makefile:8,48` |
| XP-209 | `program kdensity`; `STATISTICS::get_sd`; `TOOLS::linspace`; local `gaussian_kernel`; `PARSE_CMD` | DEP-001, DEP-003, DEP-004 | E3 `numutils/src/kdensity.f90:1-6,88,113`; `numutils/src/Makefile:8,32-34` |
| XP-210 | `program numstat`; `STATISTICS::get_moments`; `D_UNORDERED_LIST`; `PARSE_CMD` | DEP-001, DEP-003, DEP-004 | E3 `numutils/src/numstat.f90:1-5,125`; `numutils/src/Makefile:8,76-78` |
| XP-211 | `program plot_3d`; `SLPLOT::splot3d`; `TOOLS::linspace`; `PARSE_CMD`; gnuplot via `splot3d` | DEP-001, DEP-003, DEP-004, DEP-025, DEP-027 | E3 `numutils/src/splot.f90:1-6,74-75,143-151`; `numutils/src/Makefile:8,80-82`; `src/slplot_splot_3d.f90:38-66` |
| XP-212 | `program func`; `evaluator_create_`/`evaluator_evaluate_x_`; `D_UNORDERED_LIST`; `PARSE_CMD`; `-lmatheval` | DEP-001, DEP-003, DEP-004, DEP-026 | E3 `numutils/src/func.f90:1-12,58-60`; `numutils/src/Makefile:8,44-46` |
| XP-213 | `program linsp` (`wmatsubara`); `TOOLS::arange`; `PARSE_CMD`; `COMMON_VARS` `pi` | DEP-001, DEP-003, DEP-004 | E3 `numutils/src/wmatsubara.f90:1-4,31`; `numutils/src/Makefile:8,52-54` |
| XP-214 | `program fftgf_`; `FFTGF` `cfft_1d_forward`/`backward`/`fftgf_*`; `Z_UNORDERED_LIST`; `PARSE_CMD`; current NR backend | DEP-001, DEP-003, DEP-004, DEP-014, DEP-021 | E3 `numutils/src/fftgf.f90:1-6,90-104`; `numutils/src/Makefile:8,40-42`; `src/FFTGF.f90` |
| XP-215 | `program arange_`; `TOOLS::arange`; `PARSE_CMD` | DEP-001, DEP-003, DEP-004 | E3 `numutils/src/arange.f90:1-4,26`; `numutils/src/Makefile:8,56-58` |
| XP-216 | `program pade_`; `PADE::pade_analytic_continuation`; `TOOLS::linspace`; `IOTOOLS` `splot`; `PARSE_CMD` | DEP-001, DEP-003, DEP-004 | E3 `numutils/src/pade.f90:1-6,62-64`; `numutils/src/Makefile:8,28-30` |
| XP-217 | `program logsp`; `TOOLS::logspace`; `PARSE_CMD` | DEP-001, DEP-003, DEP-004 | E3 `numutils/src/logspace.f90:1-4,46`; `numutils/src/Makefile:8,64-66` |
| XP-218 | `program linsp` (`linspace`); `TOOLS::linspace`; `PARSE_CMD` | DEP-001, DEP-003, DEP-004 | E3 `numutils/src/linspace.f90:1-4,46`; `numutils/src/Makefile:8,60-62` |
| XP-219 | `program fermi_`; `FUNCTIONS::fermi`; `D_UNORDERED_LIST`; `PARSE_CMD` | DEP-001, DEP-003, DEP-004 | E3 `numutils/src/fermi.f90:1-7,47`; `numutils/src/Makefile:8,68-70` |
| XP-220 | `program spline_`; `SPLINE::cubic_spline`/`poly_spline`/`linear_spline`; `TOOLS::linspace`; `PARSE_CMD` | DEP-001, DEP-003, DEP-004, DEP-020, DEP-021 | E3 `numutils/src/spline.f90:1-6,82-86`; `numutils/src/Makefile:8,72-74` |
| XP-221 | `program random_`; `RANDOM::init_random_number`; then Fortran `random_number` (not `nrand`/`rand`) | DEP-001, DEP-003, DEP-004 | E3 `numutils/src/random.f90:1-4,48-52`; `numutils/src/Makefile:8,16-18` |
| XP-222 | `program histogram_`; `STATISTICS` histogram allocate/set/accumulate/print; `D_ORDERED_LIST`; `PARSE_CMD` | DEP-001, DEP-003, DEP-004 | E3 `numutils/src/histogram.f90:1-5,74-81`; `numutils/src/Makefile:8,36-38` |
| XP-223 | `scripts/build.sh`: `source sciforvars.sh`; default `FC=gfortran`; `select_fft_backend` (`NR`/`FFTW3`/`MKL`); `make` in `src/`; link `fidelity/driver.f90` `-lscifor -lopenblas -lgfortran` | DEP-001, DEP-002, DEP-003, DEP-004, DEP-005, DEP-006, DEP-007, DEP-009, DEP-010, DEP-011, DEP-012, DEP-013, DEP-014 | E3 `scripts/build.sh:1-99`; `bin/sciforvars.sh:22`; `etc/library.conf:1` |
| XP-224 | `scripts/fidelity.sh` runs `.bin/scifor-fidelity`; `python3` numeric compare vs `fidelity/golden/` | DEP-007, DEP-008 | E1 `scripts/fidelity.sh:1-11,32-68,161-184`; oracle probe |
| XP-225 | `program scifor_fidelity`; `USE TOOLS` (`linspace`/`logspace`/`deriv`); `USE FUNCTIONS` (`fermi`); arange-labeled loop is `real(i,8)` not `TOOLS::arange`; compiled by XP-223 against `.mod`/`-lscifor` | DEP-001, DEP-003 | E1 `fidelity/driver.f90:1-67`; `scripts/build.sh:72-76` |
| XP-226 | file-scope Zhang/Jin pack `include functions_special_funcs.f90` before `MODULE FUNCTIONS` (compiles into `FUNCTIONS.o`; not `public ::`) | DEP-001, DEP-018 | E3 `src/FUNCTIONS.f90:171`; `src/functions_special_funcs.f90`; `src/Makefile:120-122` |
| XP-227 | `src/CHRPACK.f90` file-scope character pack; not an `allmod` target and not `INCLUDE`d by compiled modules | DEP-024 | E3 `src/CHRPACK.f90` exists; `src/Makefile:6` `allmod` omits CHRPACK |
| XP-228 | `m_uniinv::uniinv` in `src/uniinv.f90`; not a Makefile target | none | E3 `src/uniinv.f90:10-22`; absent from `src/Makefile:6` |
| XP-229 | `M_UNISTA::unista` in `src/unista.f90`; `USE M_UNIINV`; not a Makefile target | none | E3 `src/unista.f90:4-13`; absent from `src/Makefile:6` |
| XP-230 | file-scope ACM 712 pack `src/random_routines.f90`; `RANDOM` include commented out; not a Makefile target | DEP-023 | E3 `src/RANDOM.f90:291`; `src/random_routines.f90:7-11`; absent from `src/Makefile:6` |
| XP-231 | `FFTGF::cfft_1d_ex` on `FFTGF_MKL.f90` (`Dfti*`) and `FFTGF_FFTW3.f90` (`dfftw_*`); not on current `FFTGF.f90` → `FFTGF_NR.f90`; selectable via `scripts/build.sh` | DEP-001, DEP-012, DEP-013 | E3 `src/FFTGF_MKL.f90:11,66-75`; `src/FFTGF_FFTW3.f90:5,57-66`; `src/FFTGF.f90:6`; `scripts/build.sh:50-62` |
| XP-232 | `FFTGF_FFTPACK` `cfft_1d_forward`/`backward` call `zffti`/`zfftf`/`zfftb`; not an `allmod` target; not an `FFT_BACKEND` choice | DEP-015 | E3 `src/FFTGF_FFTPACK.f90:1-6,15-31`; `src/Makefile:106-108`; `scripts/build.sh:50-62` |
| XP-233 | `bin/setup_sf.sh` interactive installer: compiler prompt `gfortran`/`ifort`; MKL path; `ln` missing `FFT_MKL.f90`/`FFT_FFTW3.f90`/`FFT_NR.f90`; `source` missing `mylibvars.sh`; `make` | DEP-001, DEP-002, DEP-003, DEP-004, DEP-007, DEP-009, DEP-010, DEP-012, DEP-013, DEP-014, DEP-028 | E3 `bin/setup_sf.sh:7-40,49-70,193-205`; `include/options.inc:1-7` |
| XP-234 | `program vfplot`; `USE DLPLOT` (module absent from tree); `USE VECTORS`/`TOOLS`/`COMMON_VARS`/`D_UNORDERED_LIST`; Makefile `${DSL_LIBS}` (DISLIN/DLPLOT block commented in `sfmake.inc`) | DEP-001, DEP-003, DEP-004 | E3 `numutils/src/vfplot.f90:1-7,65-66`; `numutils/src/Makefile:8,84-86`; `include/sfmake.inc:57-65` |
| XP-235 | `program ffcmplx`; `SLREAD::sread`/`SLPLOT::splot`; `SYSTEM("rm -f …")`; not in `all` | DEP-001, DEP-003, DEP-004, DEP-027 | E3 `numutils/src/ffcmplx.f90:1-4,50-54`; `numutils/src/Makefile:8,24-26` |
| XP-236 | `numutils/src/Makefile` target `diagPAM` compiles `dfftpack.o` plus missing `diagPAM.f90`; not in `all` | DEP-001, DEP-003, DEP-004, DEP-015 | E4 `numutils/src/Makefile:8,20-22`; `dfftpack.o`/`diagPAM.f90` absent; `dfftpack` mapped to FFTPACK (DEP-015) by name |
| XP-237 | `STRING_PACK` not present in this tree | none | E4 git `2915e6b` (“STRING_PACK has been moved to .repo (never really used)”); no source in checkout `e586903` |

## 2. Reverse index: dependency to paths

| DEP ID | Used by XP IDs | Evidence |
|--------|----------------|----------|
| DEP-001 | XP-001, XP-003, XP-004, XP-005, XP-006, XP-007, XP-008, XP-009, XP-010, XP-011, XP-012, XP-013, XP-014, XP-015, XP-016, XP-017, XP-018, XP-019, XP-020, XP-021, XP-022, XP-023, XP-024, XP-025, XP-026, XP-027, XP-028, XP-029, XP-030, XP-031, XP-032, XP-033, XP-034, XP-035, XP-036, XP-037, XP-038, XP-039, XP-040, XP-041, XP-042, XP-043, XP-044, XP-045, XP-046, XP-047, XP-048, XP-049, XP-050, XP-051, XP-052, XP-053, XP-054, XP-055, XP-056, XP-057, XP-058, XP-059, XP-060, XP-061, XP-062, XP-063, XP-064, XP-065, XP-066, XP-067, XP-068, XP-069, XP-070, XP-071, XP-072, XP-073, XP-074, XP-075, XP-076, XP-077, XP-078, XP-079, XP-080, XP-081, XP-082, XP-083, XP-084, XP-085, XP-086, XP-087, XP-088, XP-089, XP-090, XP-091, XP-092, XP-093, XP-094, XP-095, XP-096, XP-097, XP-098, XP-099, XP-100, XP-101, XP-102, XP-103, XP-104, XP-105, XP-106, XP-107, XP-108, XP-109, XP-110, XP-111, XP-112, XP-113, XP-114, XP-115, XP-116, XP-117, XP-118, XP-119, XP-120, XP-121, XP-122, XP-123, XP-124, XP-125, XP-126, XP-127, XP-128, XP-129, XP-130, XP-131, XP-132, XP-133, XP-134, XP-135, XP-136, XP-137, XP-138, XP-139, XP-140, XP-141, XP-142, XP-143, XP-144, XP-145, XP-146, XP-147, XP-148, XP-149, XP-150, XP-151, XP-152, XP-153, XP-154, XP-155, XP-156, XP-157, XP-158, XP-159, XP-160, XP-161, XP-162, XP-163, XP-164, XP-165, XP-166, XP-167, XP-168, XP-169, XP-170, XP-171, XP-172, XP-173, XP-174, XP-175, XP-176, XP-177, XP-178, XP-179, XP-180, XP-181, XP-182, XP-183, XP-184, XP-185, XP-186, XP-187, XP-188, XP-189, XP-190, XP-191, XP-192, XP-193, XP-194, XP-195, XP-196, XP-197, XP-198, XP-199, XP-200, XP-201, XP-202, XP-203, XP-204, XP-205, XP-206, XP-207, XP-208, XP-209, XP-210, XP-211, XP-212, XP-213, XP-214, XP-215, XP-216, XP-217, XP-218, XP-219, XP-220, XP-221, XP-222, XP-223, XP-225, XP-226, XP-231, XP-233, XP-234, XP-235, XP-236 | E3 `include/options.inc:9-16`; `scripts/build.sh:85-86`; `src/Makefile` `$(FC)` recipes; every compiled Fortran unit/CLI actually built |
| DEP-002 | XP-001, XP-002, XP-223, XP-233 | E3 `include/options.inc:1-7`; `etc/library.conf:1`; `bin/setup_sf.sh:33-41`; `scripts/build.sh:39-40` sources then may override `FC` |
| DEP-003 | XP-001, XP-002, XP-208, XP-209, XP-210, XP-211, XP-212, XP-213, XP-214, XP-215, XP-216, XP-217, XP-218, XP-219, XP-220, XP-221, XP-222, XP-223, XP-225, XP-233, XP-234, XP-235, XP-236 | E3 `src/Makefile:8-12`; `bin/sciforvars.sh:12-13`; `numutils/src/Makefile` `${SFMODS} ${SFLIBS}`; `scripts/build.sh:74-76`; `etc/Makefile.template` consumer link (inventory) |
| DEP-004 | XP-001, XP-208, XP-209, XP-210, XP-211, XP-212, XP-213, XP-214, XP-215, XP-216, XP-217, XP-218, XP-219, XP-220, XP-221, XP-222, XP-223, XP-233, XP-234, XP-235, XP-236 | E3 `src/Makefile`; `numutils/src/Makefile`; `scripts/build.sh:80`; `bin/setup_sf.sh:205` |
| DEP-005 | XP-001, XP-223 | E3 `src/Makefile:9-12,16-19`; `scripts/build.sh:81` `require_cmd rsync` |
| DEP-006 | XP-001, XP-003, XP-223 | E3 `src/Makefile:2,24-25`; `src/COMVARS.f90:9,114-116` |
| DEP-007 | XP-002, XP-223, XP-224, XP-233 | E3 `bin/sciforvars.sh:1`; `scripts/build.sh:1`; `scripts/fidelity.sh:1`; `bin/setup_sf.sh:1` |
| DEP-008 | XP-224 | E1 `scripts/fidelity.sh:32` `python3`; oracle probe |
| DEP-009 | XP-171, XP-172, XP-173, XP-174, XP-175, XP-176, XP-178, XP-223, XP-233 | E3 `src/MATRIX.f90` LAPACK calls sit on a BLAS-capable link; `include/sfmake.inc:18-20`; `scripts/build.sh:75`; `bin/setup_sf.sh` `SFBLAS` |
| DEP-010 | XP-171, XP-172, XP-173, XP-174, XP-175, XP-176, XP-178, XP-223, XP-233 | E3 `src/MATRIX.f90:99-127,164-245,320-404,443-530`; `include/sfmake.inc:11-20`; `scripts/build.sh:35,75` |
| DEP-011 | XP-223 | E3 `scripts/build.sh:34,75` `-lopenblas` / `SFBLAS`; not attached to library symbols that do not call BLAS |
| DEP-012 | XP-171, XP-172, XP-173, XP-174, XP-175, XP-176, XP-178, XP-223, XP-231, XP-233 | E3 `src/MATRIX.f90:10` `include mkl_lapack.fi`; `src/FFTGF_MKL.f90:1-35`; `scripts/build.sh:54`; `bin/setup_sf.sh:51-67,196-197` |
| DEP-013 | XP-223, XP-231, XP-233 | E3 `src/FFTGF_FFTW3.f90:3,20-23`; `scripts/build.sh:54`; `bin/setup_sf.sh:198-199`; `include/sfmake.inc:28-32` |
| DEP-014 | XP-094, XP-095, XP-096, XP-097, XP-098, XP-099, XP-100, XP-101, XP-102, XP-103, XP-104, XP-105, XP-106, XP-107, XP-214, XP-223, XP-233 | E3 `src/FFTGF.f90` symlink / `FFTGF_NR.f90:31,426`; `scripts/build.sh:7,53`; `bin/setup_sf.sh:201`; `numutils/src/fftgf.f90:92-104` |
| DEP-015 | XP-232, XP-236 | E4 `src/FFTGF_FFTPACK.f90:20-31` `zffti`/`zfftf`/`zfftb` (E3); `numutils/src/Makefile:20-22` `dfftpack.o` mapped to FFTPACK by name (E4 weakest on XP-236 join) |
| DEP-016 | XP-121 | E3 `src/INTEGRATE.f90:345`; `src/Makefile:150`; `src/integrate_d_quadpack.f90:197,2502` |
| DEP-017 | XP-092, XP-093 | E3 `src/WRAP_MINPACK.f90:1,20,36`; `src/minpack.f90:1383` |
| DEP-018 | XP-226 | E3 `src/FUNCTIONS.f90:171`; `src/functions_special_funcs.f90` Zhang/Jin copyright block |
| DEP-019 | none | E3 `src/SPLINE.f90:3` includes `spline_interp.f90`; grep finds `cc_abscissas`/`lagrange_value` only inside that include — no recovered public path calls them |
| DEP-020 | XP-089, XP-090, XP-091, XP-121, XP-128, XP-177, XP-179, XP-180, XP-186, XP-220 | E3 `src/BROYDEN.f90`+`broydn_routines.f90`; `src/BRENT.f90:23-29`; `src/INTEGRATE.f90:366,451,479`; `src/MATRIX.f90:58-61,555`; `src/spline_nr_mod.f90:11,42`; `src/spline_finter_mod.f90:21-29`; `numutils/src/spline.f90:84` |
| DEP-021 | XP-101, XP-103, XP-181, XP-185, XP-214, XP-220 | E3 `src/SPLINE.f90:4,178-181,236-239`; `src/splinefft.f90:16-19` included from `FFTGF.f90:264,288`; `numutils/src/spline.f90:82` |
| DEP-022 | XP-112, XP-113 | E3 `src/FUNCTIONS.f90:284-302` |
| DEP-023 | XP-230 | E3 `src/random_routines.f90:7-11`; `src/RANDOM.f90:291` include commented — live `RANDOM` APIs do not call ACM 712 |
| DEP-024 | XP-227 | E3 `src/CHRPACK.f90`; `src/Makefile:6` omits CHRPACK |
| DEP-025 | XP-056, XP-211 | E3 `src/slplot_splot_3d.f90:39-40`; `numutils/src/splot.f90:31-34,143` |
| DEP-026 | XP-212 | E3 `numutils/src/func.f90:11-12,58-60`; `numutils/src/Makefile:46` |
| DEP-027 | XP-041, XP-043, XP-044, XP-050, XP-051, XP-056, XP-077, XP-085, XP-132, XP-211, XP-235 | E3 `src/IOFILE.f90:230,266,290`; `src/tools_bethe.f90:19`; `src/TOOLS.f90:273-274`; `src/SQUARE_LATTICE.f90:159-160`; `src/slplot_splot_3d.f90:66`; `numutils/src/ffcmplx.f90:52` |
| DEP-028 | XP-001, XP-233 | E3 `include/options.inc:2` ifort `OPT` `-openmp`; `src/COMVARS.f90:67` `omp_*` globals; no `$OMP`/`omp_get_*` in `src/`; `bin/setup_sf.sh` may select ifort |
| DEP-029 | XP-003, XP-004, XP-005, XP-006, XP-007, XP-008, XP-036, XP-037, XP-038, XP-039, XP-050, XP-051, XP-060, XP-061 | E3 `src/COMVARS.f90:61,114,141,198-241`; `src/timer_bar.f90:9`; `src/timer_chrono.f90:6,33,140`; `src/TOOLS.f90:130,152`; `src/IOFILE.f90:289` |

## 4. Open graph questions

- [ ] When `FFT_BACKEND` is `FFTW3` or `MKL`, do XP-094–XP-104 rebind from DEP-014 to DEP-013/DEP-012 while keeping the same XP IDs? Current tree symlink is NR (`src/FFTGF.f90` → `FFTGF_NR.f90`); XP-223 records the switch.
- [ ] Should `USE DLPLOT` on XP-234 be inventoried as DISLIN/`libdlplot`? `include/sfmake.inc:57-65` DISLIN block is commented; inventory omitted those as not current; this graph treats `DLPLOT` as a missing internal module, not a new DEP ID.
- [ ] Is `dfftpack.o` on XP-236 the same FFTPACK family as DEP-015 (`zffti`/`zfftf`/`zfftb`)? Join is E4 by filename; object and `diagPAM.f90` are absent.
- [ ] Do any consumers call Burkardt `spline_interp.f90` (`cc_abscissas`/`lagrange_value`) via `EXTERNAL`? Public `SPLINE` APIs recovered here do not.
- [ ] `COMMON_VARS` public `omp_num_threads`/`omp_id`/`omp_size` (`src/COMVARS.f90:67`) have no procedure XP; DEP-028 is joined only to ifort `-openmp` build/config paths.

## Errata

None.

## Links

- Path inventory: `docs/modernization/execution-path-inventory.md`
- Dependency inventory: `docs/modernization/dependency-inventory.md`
- Migration plan: `docs/modernization/migration-plan.md`
