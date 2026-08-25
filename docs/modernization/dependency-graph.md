# Dependency Graph

**Legacy repo:** `/Users/philipteitel/code/ADD-migrations/sci-fortran-legacy`
**Version:** v2
**Status:** Snapshot
**Date:** 2026-08-25
**Owner:** Archaeologist (`/map-dependency-graph`)

## Summary

This graph joins execution-path inventory v1 (XP-001–XP-400) to internal Fortran modules/symbols and to dependency-inventory v2 DEP IDs for sci-fortran-legacy (checkout e586903; default FFT backend NR). v2 adds the XP-111 → DEP-029 edge (Alan Miller `dcerf` in `functions_zerf.f90`); the library build XP-386 compiles that unit, matching how DEP-013 is joined. Dependency inventory v2 records zero `undecided` and zero `no-route` rows; this file does not copy those dispositions as new state. Edges are procedure-level (what the path body calls or the job invokes), not unused module USE lists. Most library APIs use only DEP-001 (gfortran/language runtime). Widely shared non-runtime DEPs are DEP-009 (XP-205–XP-369 Zhang/Jin), DEP-006 (NR FFT/Broyden/Brent/polint/ran2/GJ), and DEP-003 (MATRIX plus the fidelity/build link). A first slice can start at DEP-001-only numeric APIs (for example TOOLS::linspace/logspace/arange, FUNCTIONS::fermi, TOOLS::deriv); sequencing belongs in the migration plan. Look next at `/analyze-impedance`.

---

## 1. Path to internals and externals

| XP ID | Internal modules / functions | External DEP IDs | Evidence |
|-------|------------------------------|------------------|----------|
| XP-001 | COMMON_VARS::version; include scifor_version.inc (sf_version); COMMON_VARS::timestamp, bg_green | DEP-001, DEP-018 | E3 src/COMVARS.f90:9,112-124; src/Makefile:2,24-25 |
| XP-002 | COMMON_VARS::timestamp (date_and_time intrinsic) | DEP-001 | E3 src/COMVARS.f90:93,136-145 |
| XP-003 | COMMON_VARS::abort (alias of error) | DEP-001 | E3 src/COMVARS.f90:86-88,94,192-209 |
| XP-004 | COMMON_VARS::error | DEP-001 | E3 src/COMVARS.f90:94,192-209 |
| XP-005 | COMMON_VARS::warning | DEP-001 | E3 src/COMVARS.f90:94,211-227 |
| XP-006 | COMMON_VARS::msg (MPIID stub gate; MPI not linked) | DEP-001 | E3 src/COMVARS.f90:59-62,95,235-254 |
| XP-007 | COMMON_VARS::txtfy (i_to_ch, r_to_ch, c_to_ch) | DEP-001 | E3 src/COMVARS.f90:82-84,96 |
| XP-008 | COMMON_VARS::bold (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:97 |
| XP-009 | COMMON_VARS::underline (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:98 |
| XP-010 | COMMON_VARS::highlight (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:99 |
| XP-011 | COMMON_VARS::erased (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:100 |
| XP-012 | COMMON_VARS::red (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:101 |
| XP-013 | COMMON_VARS::green (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:101 |
| XP-014 | COMMON_VARS::yellow (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:101 |
| XP-015 | COMMON_VARS::blue (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:101 |
| XP-016 | COMMON_VARS::purple (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:101 |
| XP-017 | COMMON_VARS::cyan (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:101 |
| XP-018 | COMMON_VARS::bold_red (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:102 |
| XP-019 | COMMON_VARS::bold_green (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:102 |
| XP-020 | COMMON_VARS::bold_yellow (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:102 |
| XP-021 | COMMON_VARS::bold_blue (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:102 |
| XP-022 | COMMON_VARS::bold_purple (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:102 |
| XP-023 | COMMON_VARS::bold_cyan (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:102 |
| XP-024 | COMMON_VARS::bg_red (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:103 |
| XP-025 | COMMON_VARS::bg_green (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:103 |
| XP-026 | COMMON_VARS::bg_yellow (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:103 |
| XP-027 | COMMON_VARS::bg_blue (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:103 |
| XP-028 | COMMON_VARS::bg_purple (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:103 |
| XP-029 | COMMON_VARS::bg_cyan (ANSI string wrap) | DEP-001 | E3 src/COMVARS.f90:103 |
| XP-030 | PARSE_CMD::parse_cmd_variable; COMMON_VARS::msg; command_argument_count/get_command_argument | DEP-001 | E3 src/PARSECMD.f90:6,18-23,109-128 |
| XP-031 | PARSE_CMD::get_cmd_variable; command_argument_count/get_command_argument | DEP-001 | E3 src/PARSECMD.f90:24,87-100 |
| XP-032 | PARSE_CMD::parse_cmd_help; COMMON_VARS::msg | DEP-001 | E3 src/PARSECMD.f90:25,36-61 |
| XP-033 | PARSE_CMD::print_cmd_help | DEP-001 | E3 src/PARSECMD.f90:26,70-78 |
| XP-034 | TIMER::print_bar; include timer_bar.f90; COMMON_VARS | DEP-001 | E3 src/TIMER.f90:8,35,43; src/timer_bar.f90:6 |
| XP-035 | TIMER::start_timer; include timer_chrono.f90; COMMON_VARS::error | DEP-001 | E3 src/TIMER.f90:8,36,41; src/timer_chrono.f90:5 |
| XP-036 | TIMER::stop_timer; include timer_chrono.f90 | DEP-001 | E3 src/TIMER.f90:36,41; src/timer_chrono.f90:31 |
| XP-037 | TIMER::eta; include timer_chrono.f90 | DEP-001 | E3 src/TIMER.f90:37,41; src/timer_chrono.f90:126 |
| XP-038 | IOFILE::file_size; COMMON_VARS::msg; GNU fstat | DEP-001 | E3 src/IOFILE.f90:8,31,102-118 |
| XP-039 | IOFILE::file_length; may call IOFILE::data_open | DEP-001, DEP-015 | E3 src/IOFILE.f90:32,174-203,249-266 |
| XP-040 | IOFILE::file_info; GNU fstat | DEP-001 | E3 src/IOFILE.f90:33,131-161 |
| XP-041 | IOFILE::data_open; COMMON_VARS::msg; SYSTEM gunzip | DEP-001, DEP-015, DEP-016 | E3 src/IOFILE.f90:34,249-266 |
| XP-042 | IOFILE::data_store; IOFILE::file_size; SYSTEM gzip | DEP-001, DEP-015, DEP-016 | E3 src/IOFILE.f90:35,219-232 |
| XP-043 | IOFILE::set_store_size; COMMON_VARS::warning, txtfy | DEP-001 | E3 src/IOFILE.f90:36,234-238 |
| XP-044 | IOFILE::reg_filename | DEP-001 | E3 src/IOFILE.f90:37,49-53 |
| XP-045 | IOFILE::reg (alias of reg_filename) | DEP-001 | E3 src/IOFILE.f90:15-17,37,49-53 |
| XP-046 | IOFILE::txtfit (alias of reg_filename) | DEP-001 | E3 src/IOFILE.f90:19-21,37 |
| XP-047 | IOFILE::txtcut (alias of reg_filename) | DEP-001 | E3 src/IOFILE.f90:23-25,37 |
| XP-048 | IOFILE::create_data_dir; SYSTEM mkdir; COMMON_VARS MPIID stub | DEP-001, DEP-016 | E3 src/IOFILE.f90:38,281-292 |
| XP-049 | IOFILE::create_dir (alias of create_data_dir) | DEP-001, DEP-016 | E3 src/IOFILE.f90:27-29,38,281-292 |
| XP-050 | IOFILE::close_file; IOFILE::reg | DEP-001 | E3 src/IOFILE.f90:39,77-82 |
| XP-051 | IOFILE::get_filename | DEP-001 | E3 src/IOFILE.f90:40,55-64 |
| XP-052 | IOFILE::get_filepath | DEP-001 | E3 src/IOFILE.f90:41,66-75 |
| XP-053 | SLPLOT::splot; includes slplot_splot_{P,V,M}.f90; IOFILE; COMMON_VARS (file write, no gnuplot host) | DEP-001 | E3 src/SLPLOT.f90:7-8,12-25,41,49-55 |
| XP-054 | SLPLOT::splot3d; include slplot_splot_3d.f90; SYSTEM chmod; writes gnuplot -persist scripts | DEP-001, DEP-016, DEP-017 | E3 src/SLPLOT.f90:28-31,42,58; src/slplot_splot_3d.f90:39-66 |
| XP-055 | SLPLOT::store_data; include slplot_data_save.f90; IOFILE::data_store | DEP-001, DEP-015, DEP-016 | E3 src/SLPLOT.f90:34-39,43,61; src/slplot_data_save.f90:11 |
| XP-056 | SLREAD::sread; includes slread_sread_{P,V,M}.f90; IOFILE; COMMON_VARS | DEP-001 | E3 src/SLREAD.f90:7-8,13-29,41,46-52 |
| XP-057 | SLREAD::read_data; include slread_data_read.f90 / data_read_control.f90; IOFILE; COMMON_VARS::msg | DEP-001 | E3 src/SLREAD.f90:31-39,42,55; src/data_read_control.f90:4 |
| XP-058 | TOOLS::start_loop; TIMER::start_timer; COMMON_VARS::bold | DEP-001 | E3 src/TOOLS.f90:7-8,14,121-140 |
| XP-059 | TOOLS::end_loop; TIMER::stop_timer; COMMON_VARS::bold | DEP-001 | E3 src/TOOLS.f90:15,147-158 |
| XP-060 | TOOLS::linspace; include tools_grids.f90; COMMON_VARS::error | DEP-001 | E3 src/TOOLS.f90:18,169; src/tools_grids.f90:1-29 |
| XP-061 | TOOLS::logspace; TOOLS::linspace; COMMON_VARS::error | DEP-001 | E3 src/TOOLS.f90:19; src/tools_grids.f90:32-47 |
| XP-062 | TOOLS::arange; COMMON_VARS::error | DEP-001 | E3 src/TOOLS.f90:20; src/tools_grids.f90:51-63 |
| XP-063 | TOOLS::powspace; include tools_grids.f90 | DEP-001 | E3 src/TOOLS.f90:21; src/tools_grids.f90:147 |
| XP-064 | TOOLS::upmspace; include tools_grids.f90 | DEP-001 | E3 src/TOOLS.f90:22; src/tools_grids.f90:96-147 |
| XP-065 | TOOLS::upminterval; TOOLS::upmspace | DEP-001 | E3 src/TOOLS.f90:23; src/tools_grids.f90:68-92 |
| XP-066 | TOOLS::sort; include tools_sort1d.f90 (insertion sort) | DEP-001 | E3 src/TOOLS.f90:26,221; src/tools_sort1d.f90:70-85 |
| XP-067 | TOOLS::sort_array; qsort_sort in tools_sort1d.f90 | DEP-001 | E3 src/TOOLS.f90:26; src/tools_sort1d.f90:94-114 |
| XP-068 | TOOLS::uniq; TOOLS::unista (inlined ORDERPACK) | DEP-001, DEP-010 | E3 src/TOOLS.f90:27,66-69,221; src/tools_sort1d.f90:8-24 |
| XP-069 | TOOLS::reshuffle; include tools_sort1d.f90 | DEP-001 | E3 src/TOOLS.f90:27; src/tools_sort1d.f90:51-59 |
| XP-070 | TOOLS::shiftFW; include tools_shifts.f90 | DEP-001 | E3 src/TOOLS.f90:28,49-51,222; src/tools_shifts.f90:7-43 |
| XP-071 | TOOLS::shiftBW; include tools_shifts.f90 | DEP-001 | E3 src/TOOLS.f90:28,53-55; src/tools_shifts.f90:72-108 |
| XP-072 | TOOLS::deriv | DEP-001 | E3 src/TOOLS.f90:31,175-186 |
| XP-073 | TOOLS::gfbethe; include tools_bethe.f90 | DEP-001 | E3 src/TOOLS.f90:34,208; src/tools_bethe.f90:65-75 |
| XP-074 | TOOLS::gfbether; include tools_bethe.f90 | DEP-001 | E3 src/TOOLS.f90:35; src/tools_bethe.f90:86-94 |
| XP-075 | TOOLS::bethe_lattice; TOOLS::dens_bethe; SYSTEM mkdir | DEP-001, DEP-016 | E3 src/TOOLS.f90:36; src/tools_bethe.f90:7-28 |
| XP-076 | TOOLS::dens_bethe | DEP-001 | E3 src/TOOLS.f90:37; src/tools_bethe.f90:41-51 |
| XP-077 | TOOLS::dens_hyperc | DEP-001 | E3 src/TOOLS.f90:38,193-202 |
| XP-078 | TOOLS::find2Dmesh | DEP-001 | E3 src/TOOLS.f90:41,328 |
| XP-079 | TOOLS::get_matsubara_gf_from_dos | DEP-001 | E3 src/TOOLS.f90:44,285-312 |
| XP-080 | TOOLS::check_convergence; includes tools_check_function1d.f90, tools_write_error_file_*, tools_print_error_msg_* | DEP-001 | E3 src/TOOLS.f90:45,88-99,215 |
| XP-081 | TOOLS::check_convergence_scalar; include tools_check_scalar.f90 | DEP-001 | E3 src/TOOLS.f90:45,75-86,214 |
| XP-082 | TOOLS::check_convergence_local; include tools_check_function1d_local.f90 | DEP-001 | E3 src/TOOLS.f90:45,101-112,216 |
| XP-083 | TOOLS::get_free_dos; SYSTEM mkdir/mv | DEP-001, DEP-016 | E3 src/TOOLS.f90:46,244-276 |
| XP-084 | TOOLS::sum_overk_zeta | DEP-001 | E3 src/TOOLS.f90:47,231-238 |
| XP-085 | TOOLS::uniinv (ORDERPACK inlined in TOOLS.f90) | DEP-001, DEP-010 | E3 src/TOOLS.f90:57-60,417 |
| XP-086 | TOOLS::unista; TOOLS::uniinv | DEP-001, DEP-010 | E3 src/TOOLS.f90:66-69,1181 |
| XP-087 | BROYDEN::broydn; BROYDN_ROUTINES / BROYDN_UTILS (include broydn_routines.f90) | DEP-001, DEP-006 | E3 src/BROYDEN.f90:1-6,29; src/broydn_routines.f90:1,215 |
| XP-088 | BRENT::fzero; BRENT::zbrent | DEP-001, DEP-006 | E3 src/BRENT.f90:5,11-23 |
| XP-089 | BRENT::zbrent | DEP-001, DEP-006 | E3 src/BRENT.f90:6,29-46 |
| XP-090 | WRAP_MINPACK::fsolve; include minpack.f90; hybrd1 | DEP-001, DEP-007 | E3 src/WRAP_MINPACK.f90:1,5,9-22; src/minpack.f90:1383 |
| XP-091 | WRAP_MINPACK::ffsolve; hybrd1 | DEP-001, DEP-007 | E3 src/WRAP_MINPACK.f90:5,24-39 |
| XP-092 | FFTGF::cfft_1d_forward; FFTGF::four1; nr_radix2_test (default NR backend) | DEP-001, DEP-006 | E3 src/FFTGF_NR.f90:6,28-32,426; scripts/build.sh:7,50-62 |
| XP-093 | FFTGF::cfft_1d_backward; FFTGF::four1 | DEP-001, DEP-006 | E3 src/FFTGF_NR.f90:6,34-38 |
| XP-094 | FFTGF::cfft_1d_shift (array index shuffle) | DEP-001 | E3 src/FFTGF_NR.f90:6,40-47 |
| XP-095 | FFTGF::swap_fftrt2rw (half-swap) | DEP-001 | E3 src/FFTGF_NR.f90:6,49-59 |
| XP-096 | FFTGF::fftgf_rw2rt; FFTGF::cfft_1d_forward, cfft_1d_shift | DEP-001, DEP-006 | E3 src/FFTGF_NR.f90:7,68-80 |
| XP-097 | FFTGF::fftgf_rt2rw; FFTGF::cfft_1d_backward | DEP-001, DEP-006 | E3 src/FFTGF_NR.f90:7,104-117 |
| XP-098 | FFTGF::fftgf_iw2tau; FFTGF::cfft_1d_backward / fftgf_rw2rt | DEP-001, DEP-006 | E3 src/FFTGF_NR.f90:8,138-190 |
| XP-099 | FFTGF::fftgf_tau2iw; include splinefft.f90 cubspl/ppvalu; FFTGF::fftgf_rt2rw | DEP-001, DEP-006, DEP-012 | E3 src/FFTGF_NR.f90:8,247-265; src/splinefft.f90:24 |
| XP-100 | FFTGF::fftff_iw2tau; FFTGF::cosft2; TOOLS::linspace; SPLINE::linear_spline | DEP-001, DEP-006, DEP-011 | E3 src/FFTGF_NR.f90:9,210-235 |
| XP-101 | FFTGF::fftff_tau2iw; include splinefft.f90 cubspl; FFTGF::fftgf_rt2rw | DEP-001, DEP-006, DEP-012 | E3 src/FFTGF_NR.f90:9,267-289 |
| XP-102 | FFTGF::fftff_iw2tau_ (direct cosine sum; no four1/cosft2) | DEP-001 | E3 src/FFTGF_NR.f90:10,192-208 |
| XP-103 | FFTGF::four1 (NR radix-2 kernel) | DEP-001, DEP-006 | E3 src/FFTGF_NR.f90:11,424-427,426-483 |
| XP-104 | FFTGF::cosft2; FFTGF::realft | DEP-001, DEP-006 | E3 src/FFTGF_NR.f90:11,306-370 |
| XP-105 | FFTGF::realft; FFTGF::four1 | DEP-001, DEP-006 | E3 src/FFTGF_NR.f90:11,372-422 |
| XP-106 | FUNCTIONS::heaviside; USE COMMON_VARS | DEP-001 | E3 src/FUNCTIONS.f90:174,179,198-208 |
| XP-107 | FUNCTIONS::step | DEP-001 | E3 src/FUNCTIONS.f90:180,214-227 |
| XP-108 | FUNCTIONS::fermi | DEP-001 | E3 src/FUNCTIONS.f90:181,240-248 |
| XP-109 | FUNCTIONS::sgn (i_sgn, d_sgn) | DEP-001 | E3 src/FUNCTIONS.f90:182-185,260-269 |
| XP-110 | FUNCTIONS::wfun; include functions_wofz.f90 WOFZ | DEP-001, DEP-013 | E3 src/FUNCTIONS.f90:188,284-294; src/functions_wofz.f90:1-18 |
| XP-111 | FUNCTIONS::zerf; include functions_zerf.f90 dcerf | DEP-001, DEP-029 | E3 src/FUNCTIONS.f90:189,301-302; src/functions_zerf.f90:1-20 |
| XP-112 | GREENFUNX::allocate_gf; include greenfunx_allocate_gf.f90; COMMON_VARS | DEP-001 | E3 src/GREENFUNX.f90:9,37-41,63,158 |
| XP-113 | GREENFUNX::deallocate_gf; include greenfunx_deallocate_gf.f90 | DEP-001 | E3 src/GREENFUNX.f90:43-47,64,159 |
| XP-114 | GREENFUNX::assignment(=); includes greenfunx_gf_identity.f90, greenfunx_scalar_identity.f90 | DEP-001 | E3 src/GREENFUNX.f90:49-56,65,170-171 |
| XP-115 | GREENFUNX::operator(+) | DEP-001 | E3 src/GREENFUNX.f90:58-61,66 |
| XP-116 | GREENFUNX::ret_component_t (contained step) | DEP-001 | E3 src/GREENFUNX.f90:67,78-96 |
| XP-117 | GREENFUNX::less_component_w (contained fermi) | DEP-001 | E3 src/GREENFUNX.f90:68,101-122 |
| XP-118 | GREENFUNX::gtr_component_w (contained fermi) | DEP-001 | E3 src/GREENFUNX.f90:69,127-148 |
| XP-119 | INTEGRATE::kramers_kronig; kkfinter; locate/polint (inlined NR); QAWCE in integrate_d_quadpack.f90 | DEP-001, DEP-006, DEP-008 | E3 src/INTEGRATE.f90:62,320-346,355-372,479-537; src/integrate_d_quadpack.f90:2497 |
| XP-120 | INTEGRATE::kronig (uniform-mesh KK; local finite-difference) | DEP-001 | E3 src/INTEGRATE.f90:63,280-310 |
| XP-121 | INTEGRATE::trapz | DEP-001 | E3 src/INTEGRATE.f90:47-52,64,78 |
| XP-122 | INTEGRATE::simps | DEP-001 | E3 src/INTEGRATE.f90:54-59,65 |
| XP-123 | INTEGRATE::init_finter | DEP-001 | E3 src/INTEGRATE.f90:31-33,67,381-394 |
| XP-124 | INTEGRATE::kill_finter | DEP-001 | E3 src/INTEGRATE.f90:35-37,68,397-412 |
| XP-125 | INTEGRATE::set_finter | DEP-001 | E3 src/INTEGRATE.f90:39-41,69,415-435 |
| XP-126 | INTEGRATE::finter_func; locate/polint (inlined NR) | DEP-001, DEP-006 | E3 src/INTEGRATE.f90:43-45,70,438-537 |
| XP-127 | SQUARE_LATTICE::square_lattice_dispersion; VECTORS (include VECTORS.f90); COMMON_VARS | DEP-001 | E3 src/SQUARE_LATTICE.f90:7-10,37,56-68 |
| XP-128 | SQUARE_LATTICE::square_lattice_velocity; VECTORS | DEP-001 | E3 src/SQUARE_LATTICE.f90:38 |
| XP-129 | SQUARE_LATTICE::square_lattice_dimension | DEP-001 | E3 src/SQUARE_LATTICE.f90:39 |
| XP-130 | SQUARE_LATTICE::square_lattice_structure; VECTORS; COMMON_VARS::error, pi2; plot_reciprocal_lattice; SYSTEM mkdir/mv | DEP-001, DEP-016 | E3 src/SQUARE_LATTICE.f90:40,131-160 |
| XP-131 | SQUARE_LATTICE::square_lattice_dispersion_array; square_lattice_dispersion; kgrid | DEP-001 | E3 src/SQUARE_LATTICE.f90:41,270-284 |
| XP-132 | SQUARE_LATTICE::square_lattice_MGXMpath_dimension | DEP-001 | E3 src/SQUARE_LATTICE.f90:42,305-308 |
| XP-133 | SQUARE_LATTICE::square_lattice_MGXMpath_structure; VECTORS | DEP-001 | E3 src/SQUARE_LATTICE.f90:43,324 |
| XP-134 | SQUARE_LATTICE::square_lattice_reduxGrid_dimension | DEP-001 | E3 src/SQUARE_LATTICE.f90:44 |
| XP-135 | SQUARE_LATTICE::square_lattice_reduxGrid_index | DEP-001 | E3 src/SQUARE_LATTICE.f90:45 |
| XP-136 | SQUARE_LATTICE::square_lattice_reduxGrid_dispersion_array | DEP-001 | E3 src/SQUARE_LATTICE.f90:46 |
| XP-137 | VECTORS::operator(+); includes vectorial_algebra_{2d,3d}.f90 | DEP-001 | E3 src/VECTORS.f90:24-26,54,67,73 |
| XP-138 | VECTORS::operator(-) | DEP-001 | E3 src/VECTORS.f90:27-29,55 |
| XP-139 | VECTORS::operator(*) | DEP-001 | E3 src/VECTORS.f90:30-32,56 |
| XP-140 | VECTORS::operator(.dot.) | DEP-001 | E3 src/VECTORS.f90:33-35,57 |
| XP-141 | VECTORS::assignment(=) | DEP-001 | E3 src/VECTORS.f90:36-38,58 |
| XP-142 | VECTORS::modulo | DEP-001 | E3 src/VECTORS.f90:39-41,59 |
| XP-143 | D_ORDERED_LIST::init_list | DEP-001 | E3 src/LIST_D_ORDERED.f90:45 |
| XP-144 | D_ORDERED_LIST::destroy_list | DEP-001 | E3 src/LIST_D_ORDERED.f90:46 |
| XP-145 | D_ORDERED_LIST::insert_element | DEP-001 | E3 src/LIST_D_ORDERED.f90:41-43,47 |
| XP-146 | D_ORDERED_LIST::insert_element_before | DEP-001 | E3 src/LIST_D_ORDERED.f90:47 |
| XP-147 | D_ORDERED_LIST::insert_element_after | DEP-001 | E3 src/LIST_D_ORDERED.f90:47 |
| XP-148 | D_ORDERED_LIST::remove_element | DEP-001 | E3 src/LIST_D_ORDERED.f90:48 |
| XP-149 | D_ORDERED_LIST::get_value | DEP-001 | E3 src/LIST_D_ORDERED.f90:49 |
| XP-150 | D_ORDERED_LIST::get_node | DEP-001 | E3 src/LIST_D_ORDERED.f90:50 |
| XP-151 | D_ORDERED_LIST::print_list | DEP-001 | E3 src/LIST_D_ORDERED.f90:51 |
| XP-152 | D_ORDERED_LIST::dump_list | DEP-001 | E3 src/LIST_D_ORDERED.f90:51 |
| XP-153 | D_UNORDERED_LIST::init_list | DEP-001 | E3 src/LIST_D_UNORDERED.f90:17-23,27-36 |
| XP-154 | D_UNORDERED_LIST::destroy_list | DEP-001 | E3 src/LIST_D_UNORDERED.f90:17-23,27-36 |
| XP-155 | D_UNORDERED_LIST::add_element | DEP-001 | E3 src/LIST_D_UNORDERED.f90:17-23,27-36 |
| XP-156 | D_UNORDERED_LIST::remove_element | DEP-001 | E3 src/LIST_D_UNORDERED.f90:17-23,27-36 |
| XP-157 | D_UNORDERED_LIST::get_value | DEP-001 | E3 src/LIST_D_UNORDERED.f90:17-23,27-36 |
| XP-158 | D_UNORDERED_LIST::get_node | DEP-001 | E3 src/LIST_D_UNORDERED.f90:17-23,27-36 |
| XP-159 | D_UNORDERED_LIST::print_list | DEP-001 | E3 src/LIST_D_UNORDERED.f90:17-23,27-36 |
| XP-160 | D_UNORDERED_LIST::dump_list | DEP-001 | E3 src/LIST_D_UNORDERED.f90:17-23,27-36 |
| XP-161 | Z_UNORDERED_LIST::init_list | DEP-001 | E3 src/LIST_Z_UNORDERED.f90:17-23,27 |
| XP-162 | Z_UNORDERED_LIST::destroy_list | DEP-001 | E3 src/LIST_Z_UNORDERED.f90:17-23,27 |
| XP-163 | Z_UNORDERED_LIST::add_element | DEP-001 | E3 src/LIST_Z_UNORDERED.f90:17-23,27 |
| XP-164 | Z_UNORDERED_LIST::remove_element | DEP-001 | E3 src/LIST_Z_UNORDERED.f90:17-23,27 |
| XP-165 | Z_UNORDERED_LIST::get_value | DEP-001 | E3 src/LIST_Z_UNORDERED.f90:17-23,27 |
| XP-166 | Z_UNORDERED_LIST::get_node | DEP-001 | E3 src/LIST_Z_UNORDERED.f90:17-23,27 |
| XP-167 | Z_UNORDERED_LIST::print_list | DEP-001 | E3 src/LIST_Z_UNORDERED.f90:17-23,27 |
| XP-168 | Z_UNORDERED_LIST::dump_list | DEP-001 | E3 src/LIST_Z_UNORDERED.f90:17-23,27 |
| XP-169 | MATRIX::matrix_diagonalize; COMMON_VARS::error; dsyev/zheev; include mkl_lapack.fi | DEP-001, DEP-003, DEP-004 | E3 src/MATRIX.f90:7,10,46-48,63,99-127 |
| XP-170 | MATRIX::solve_linear_system; dgetrf/dgetrs/zgetrf/zgetrs; mkl_lapack.fi | DEP-001, DEP-003, DEP-004 | E3 src/MATRIX.f90:52-55,65,164-245 |
| XP-171 | MATRIX::matrix_inverse; dgetrf/dgetri/zgetrf/zgetri | DEP-001, DEP-003, DEP-004 | E3 src/MATRIX.f90:14-16,67,320-348 |
| XP-172 | MATRIX::matrix_inverse_sym; dsytrf/dsytri (complex sibling in module) | DEP-001, DEP-003, DEP-004 | E3 src/MATRIX.f90:18-20,68,443-449 |
| XP-173 | MATRIX::matrix_inverse_her; zhetrf/zhetri | DEP-001, DEP-003, DEP-004 | E3 src/MATRIX.f90:22-24,69,524-530 |
| XP-174 | MATRIX::matrix_inverse_triang; dtrtri/ztrtri | DEP-001, DEP-003, DEP-004 | E3 src/MATRIX.f90:26-28,70,386-404 |
| XP-175 | MATRIX::matrix_inverse_gj; NR90 swap helpers in MATRIX.f90 (Gauss-Jordan; no LAPACK call in GJ body; unit still includes mkl_lapack.fi) | DEP-001, DEP-003, DEP-004, DEP-006 | E3 src/MATRIX.f90:10,30-32,58-61,71 |
| XP-176 | MATRIX::m_invert (non-GJ generic → matrix_inverse LAPACK path) | DEP-001, DEP-003, DEP-004 | E3 src/MATRIX.f90:40-42,73 |
| XP-177 | MATRIX::m_invert_gj (Gauss-Jordan generic) | DEP-001, DEP-003, DEP-004, DEP-006 | E3 src/MATRIX.f90:36-38,58-61,74 |
| XP-178 | SPLINE::poly_spline; SPLINE_FINTER_MOD::finter; SPLINE_NR_MOD locate/polint | DEP-001, DEP-006 | E3 src/SPLINE.f90:1-2,13-14,18-20,34,94-124 |
| XP-179 | SPLINE::cubic_spline; CUBSPL; PPVALU (spline_cubspl_routines.f90) | DEP-001, DEP-012 | E3 src/SPLINE.f90:4,22-24,35,156-188 |
| XP-180 | SPLINE::linear_spline; Burkardt interp_linear (spline_interp.f90) | DEP-001, DEP-011 | E3 src/SPLINE.f90:3,26-28,36,47-67 |
| XP-181 | SPLINE::extract (alias of extract_gtau) | DEP-001 | E3 src/SPLINE.f90:30-32,37,262-279 |
| XP-182 | SPLINE::extract_gtau (index downsample; no CUBSPL) | DEP-001 | E3 src/SPLINE.f90:37,254-279 |
| XP-183 | SPLINE_FINTER_MOD::finter; USE SPLINE_NR_MOD locate/polint | DEP-001, DEP-006 | E3 src/spline_finter_mod.f90:1-2,9,13-32; src/spline_nr_mod.f90:1-14 |
| XP-184 | RANDOM::nrand (L'Ecuyer/NR ran2-style constants) | DEP-001, DEP-006 | E3 src/RANDOM.f90:26,145-176 |
| XP-185 | RANDOM::irand; Fortran random_number | DEP-001 | E3 src/RANDOM.f90:27,34-39 |
| XP-186 | RANDOM::drand; Fortran random_number | DEP-001 | E3 src/RANDOM.f90:27,40-43 |
| XP-187 | RANDOM::crand; Fortran random_number | DEP-001 | E3 src/RANDOM.f90:27,44-50 |
| XP-188 | RANDOM::init_random_number; RANDOM_SEED; SYSTEM_CLOCK | DEP-001 | E3 src/RANDOM.f90:28,196-207 |
| XP-189 | RANDOM::random_order | DEP-001 | E3 src/RANDOM.f90:29 |
| XP-190 | RANDOM::rand; Fortran random_number overloads | DEP-001 | E3 src/RANDOM.f90:20-24,30 |
| XP-191 | STATISTICS::histogram_allocate | DEP-001 | E3 src/STATISTICS.f90:11 |
| XP-192 | STATISTICS::histogram_set_range_uniform | DEP-001 | E3 src/STATISTICS.f90:12 |
| XP-193 | STATISTICS::histogram_accumulate | DEP-001 | E3 src/STATISTICS.f90:13 |
| XP-194 | STATISTICS::histogram_get_range | DEP-001 | E3 src/STATISTICS.f90:14 |
| XP-195 | STATISTICS::histogram_get_value | DEP-001 | E3 src/STATISTICS.f90:15 |
| XP-196 | STATISTICS::histogram_print | DEP-001 | E3 src/STATISTICS.f90:16 |
| XP-197 | STATISTICS::get_moments | DEP-001 | E3 src/STATISTICS.f90:19,27 |
| XP-198 | STATISTICS::get_mean | DEP-001 | E3 src/STATISTICS.f90:20 |
| XP-199 | STATISTICS::get_sd | DEP-001 | E3 src/STATISTICS.f90:20 |
| XP-200 | STATISTICS::get_var | DEP-001 | E3 src/STATISTICS.f90:20 |
| XP-201 | STATISTICS::get_skew | DEP-001 | E3 src/STATISTICS.f90:20 |
| XP-202 | STATISTICS::get_curt | DEP-001 | E3 src/STATISTICS.f90:20 |
| XP-203 | STATISTICS::get_covariance | DEP-001 | E3 src/STATISTICS.f90:21 |
| XP-204 | PADE::pade_analytic_continuation; COMMON_VARS::error (USE TOOLS is unused in the body) | DEP-001 | E3 src/PADE.f90:1-3,9,14-27 |
| XP-205 | file-scope airya in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:1; src/FUNCTIONS.f90:171 |
| XP-206 | file-scope airyb in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:90; src/FUNCTIONS.f90:171 |
| XP-207 | file-scope airyzo in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:319; src/FUNCTIONS.f90:171 |
| XP-208 | file-scope ajyik in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:496; src/FUNCTIONS.f90:171 |
| XP-209 | file-scope aswfa in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:807; src/FUNCTIONS.f90:171 |
| XP-210 | file-scope aswfb in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:955; src/FUNCTIONS.f90:171 |
| XP-211 | file-scope bernoa in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:1062; src/FUNCTIONS.f90:171 |
| XP-212 | file-scope bernob in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:1128; src/FUNCTIONS.f90:171 |
| XP-213 | file-scope betaf in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:1200; src/FUNCTIONS.f90:171 |
| XP-214 | file-scope bjndd in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:1255; src/FUNCTIONS.f90:171 |
| XP-215 | file-scope cbk in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:1346; src/FUNCTIONS.f90:171 |
| XP-216 | file-scope cchg in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:1511; src/FUNCTIONS.f90:171 |
| XP-217 | file-scope cerf in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:1730; src/FUNCTIONS.f90:171 |
| XP-218 | file-scope cerror in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:1870; src/FUNCTIONS.f90:171 |
| XP-219 | file-scope cerzo in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:1963; src/FUNCTIONS.f90:171 |
| XP-220 | file-scope cfc in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:2075; src/FUNCTIONS.f90:171 |
| XP-221 | file-scope cfs in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:2199; src/FUNCTIONS.f90:171 |
| XP-222 | file-scope cgama in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:2323; src/FUNCTIONS.f90:171 |
| XP-223 | file-scope ch12n in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:2467; src/FUNCTIONS.f90:171 |
| XP-224 | file-scope chgm in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:2592; src/FUNCTIONS.f90:171 |
| XP-225 | file-scope chgu in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:2776; src/FUNCTIONS.f90:171 |
| XP-226 | file-scope chgubi in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:2901; src/FUNCTIONS.f90:171 |
| XP-227 | file-scope chguit in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:3125; src/FUNCTIONS.f90:171 |
| XP-228 | file-scope chgul in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:3294; src/FUNCTIONS.f90:171 |
| XP-229 | file-scope chgus in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:3394; src/FUNCTIONS.f90:171 |
| XP-230 | file-scope cik01 in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:3493; src/FUNCTIONS.f90:171 |
| XP-231 | file-scope ciklv in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:3706; src/FUNCTIONS.f90:171 |
| XP-232 | file-scope cikna in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:3822; src/FUNCTIONS.f90:171 |
| XP-233 | file-scope ciknb in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:3961; src/FUNCTIONS.f90:171 |
| XP-234 | file-scope cikva in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:4165; src/FUNCTIONS.f90:171 |
| XP-235 | file-scope cikvb in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:4462; src/FUNCTIONS.f90:171 |
| XP-236 | file-scope cisia in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:4738; src/FUNCTIONS.f90:171 |
| XP-237 | file-scope cisib in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:4889; src/FUNCTIONS.f90:171 |
| XP-238 | file-scope cjk in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:4984; src/FUNCTIONS.f90:171 |
| XP-239 | file-scope cjy01 in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:5062; src/FUNCTIONS.f90:171 |
| XP-240 | file-scope cjylv in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:5290; src/FUNCTIONS.f90:171 |
| XP-241 | file-scope cjyna in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:5402; src/FUNCTIONS.f90:171 |
| XP-242 | file-scope cjynb in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:5662; src/FUNCTIONS.f90:171 |
| XP-243 | file-scope cjyva in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:5888; src/FUNCTIONS.f90:171 |
| XP-244 | file-scope cjyvb in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:6339; src/FUNCTIONS.f90:171 |
| XP-245 | file-scope clpmn in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:6627; src/FUNCTIONS.f90:171 |
| XP-246 | file-scope clpn in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:6758; src/FUNCTIONS.f90:171 |
| XP-247 | file-scope clqmn in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:6838; src/FUNCTIONS.f90:171 |
| XP-248 | file-scope clqn in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:7025; src/FUNCTIONS.f90:171 |
| XP-249 | file-scope comelp in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:7146; src/FUNCTIONS.f90:171 |
| XP-250 | file-scope cpbdn in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:7235; src/FUNCTIONS.f90:171 |
| XP-251 | file-scope cpdla in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:7394; src/FUNCTIONS.f90:171 |
| XP-252 | file-scope cpdsa in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:7460; src/FUNCTIONS.f90:171 |
| XP-253 | file-scope cpsi in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:7570; src/FUNCTIONS.f90:171 |
| XP-254 | file-scope csphik in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:7699; src/FUNCTIONS.f90:171 |
| XP-255 | file-scope csphjy in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:7842; src/FUNCTIONS.f90:171 |
| XP-256 | file-scope cv0 in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:7982; src/FUNCTIONS.f90:171 |
| XP-257 | file-scope cva1 in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:8359; src/FUNCTIONS.f90:171 |
| XP-258 | file-scope cva2 in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:8571; src/FUNCTIONS.f90:171 |
| XP-259 | file-scope cvf in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:8729; src/FUNCTIONS.f90:171 |
| XP-260 | file-scope cvql in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:8859; src/FUNCTIONS.f90:171 |
| XP-261 | file-scope cvqm in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:8950; src/FUNCTIONS.f90:171 |
| XP-262 | file-scope cy01 in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:9003; src/FUNCTIONS.f90:171 |
| XP-263 | file-scope cyzo in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:9252; src/FUNCTIONS.f90:171 |
| XP-264 | file-scope dvla in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:9408; src/FUNCTIONS.f90:171 |
| XP-265 | file-scope dvsa in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:9485; src/FUNCTIONS.f90:171 |
| XP-266 | file-scope e1xa in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:9589; src/FUNCTIONS.f90:171 |
| XP-267 | file-scope e1xb in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:9664; src/FUNCTIONS.f90:171 |
| XP-268 | file-scope e1z in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:9743; src/FUNCTIONS.f90:171 |
| XP-269 | file-scope eix in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:9828; src/FUNCTIONS.f90:171 |
| XP-270 | file-scope elit in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:9903; src/FUNCTIONS.f90:171 |
| XP-271 | file-scope elit3 in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:10012; src/FUNCTIONS.f90:171 |
| XP-272 | file-scope envj in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:10113; src/FUNCTIONS.f90:171 |
| XP-273 | file-scope enxa in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:10160; src/FUNCTIONS.f90:171 |
| XP-274 | file-scope enxb in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:10218; src/FUNCTIONS.f90:171 |
| XP-275 | file-scope werror in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:10332; src/FUNCTIONS.f90:171 |
| XP-276 | file-scope eulera in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:10419; src/FUNCTIONS.f90:171 |
| XP-277 | file-scope eulerb in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:10480; src/FUNCTIONS.f90:171 |
| XP-278 | file-scope fcoef in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:10551; src/FUNCTIONS.f90:171 |
| XP-279 | file-scope fcs in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:10860; src/FUNCTIONS.f90:171 |
| XP-280 | file-scope fcszo in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:11011; src/FUNCTIONS.f90:171 |
| XP-281 | file-scope ffk in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:11148; src/FUNCTIONS.f90:171 |
| XP-282 | file-scope gaih in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:11369; src/FUNCTIONS.f90:171 |
| XP-283 | file-scope gam0 in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:11430; src/FUNCTIONS.f90:171 |
| XP-284 | file-scope gammaf in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:11507; src/FUNCTIONS.f90:171 |
| XP-285 | file-scope gmn in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:11627; src/FUNCTIONS.f90:171 |
| XP-286 | file-scope herzo in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:11738; src/FUNCTIONS.f90:171 |
| XP-287 | file-scope hygfx in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:11887; src/FUNCTIONS.f90:171 |
| XP-288 | file-scope hygfz in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:12299; src/FUNCTIONS.f90:171 |
| XP-289 | file-scope ik01a in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:12818; src/FUNCTIONS.f90:171 |
| XP-290 | file-scope ik01b in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:13005; src/FUNCTIONS.f90:171 |
| XP-291 | file-scope ikna in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:13172; src/FUNCTIONS.f90:171 |
| XP-292 | file-scope iknb in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:13325; src/FUNCTIONS.f90:171 |
| XP-293 | file-scope ikv in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:13496; src/FUNCTIONS.f90:171 |
| XP-294 | file-scope incob in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:13760; src/FUNCTIONS.f90:171 |
| XP-295 | file-scope incog in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:13857; src/FUNCTIONS.f90:171 |
| XP-296 | file-scope itairy in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:13956; src/FUNCTIONS.f90:171 |
| XP-297 | file-scope itika in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:14143; src/FUNCTIONS.f90:171 |
| XP-298 | file-scope itikb in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:14282; src/FUNCTIONS.f90:171 |
| XP-299 | file-scope itjya in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:14437; src/FUNCTIONS.f90:171 |
| XP-300 | file-scope itjyb in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:14578; src/FUNCTIONS.f90:171 |
| XP-301 | file-scope itsh0 in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:14725; src/FUNCTIONS.f90:171 |
| XP-302 | file-scope itsl0 in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:14860; src/FUNCTIONS.f90:171 |
| XP-303 | file-scope itth0 in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:14979; src/FUNCTIONS.f90:171 |
| XP-304 | file-scope ittika in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:15080; src/FUNCTIONS.f90:171 |
| XP-305 | file-scope ittikb in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:15205; src/FUNCTIONS.f90:171 |
| XP-306 | file-scope ittjya in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:15338; src/FUNCTIONS.f90:171 |
| XP-307 | file-scope ittjyb in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:15509; src/FUNCTIONS.f90:171 |
| XP-308 | file-scope jdzo in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:15653; src/FUNCTIONS.f90:171 |
| XP-309 | file-scope jelp in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:15892; src/FUNCTIONS.f90:171 |
| XP-310 | file-scope jy01a in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:15987; src/FUNCTIONS.f90:171 |
| XP-311 | file-scope jy01b in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:16191; src/FUNCTIONS.f90:171 |
| XP-312 | file-scope jyna in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:16368; src/FUNCTIONS.f90:171 |
| XP-313 | file-scope jynb in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:16527; src/FUNCTIONS.f90:171 |
| XP-314 | file-scope jyndd in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:16730; src/FUNCTIONS.f90:171 |
| XP-315 | file-scope jyv in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:16848; src/FUNCTIONS.f90:171 |
| XP-316 | file-scope jyzo in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:17173; src/FUNCTIONS.f90:171 |
| XP-317 | file-scope klvna in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:17356; src/FUNCTIONS.f90:171 |
| XP-318 | file-scope klvnb in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:17628; src/FUNCTIONS.f90:171 |
| XP-319 | file-scope klvnzo in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:17889; src/FUNCTIONS.f90:171 |
| XP-320 | file-scope kmn in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:18009; src/FUNCTIONS.f90:171 |
| XP-321 | file-scope lagzo in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:18225; src/FUNCTIONS.f90:171 |
| XP-322 | file-scope lamn in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:18351; src/FUNCTIONS.f90:171 |
| XP-323 | file-scope lamv in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:18508; src/FUNCTIONS.f90:171 |
| XP-324 | file-scope legzo in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:18749; src/FUNCTIONS.f90:171 |
| XP-325 | file-scope lgama in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:18872; src/FUNCTIONS.f90:171 |
| XP-326 | file-scope lpmn in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:18969; src/FUNCTIONS.f90:171 |
| XP-327 | file-scope lpmns in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:19093; src/FUNCTIONS.f90:171 |
| XP-328 | file-scope lpmv in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:19202; src/FUNCTIONS.f90:171 |
| XP-329 | file-scope lpn in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:19401; src/FUNCTIONS.f90:171 |
| XP-330 | file-scope lpni in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:19476; src/FUNCTIONS.f90:171 |
| XP-331 | file-scope lqmn in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:19573; src/FUNCTIONS.f90:171 |
| XP-332 | file-scope lqmns in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:19751; src/FUNCTIONS.f90:171 |
| XP-333 | file-scope lqna in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:19975; src/FUNCTIONS.f90:171 |
| XP-334 | file-scope lqnb in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:20051; src/FUNCTIONS.f90:171 |
| XP-335 | file-scope msta1 in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:20190; src/FUNCTIONS.f90:171 |
| XP-336 | file-scope msta2 in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:20270; src/FUNCTIONS.f90:171 |
| XP-337 | file-scope mtu0 in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:20364; src/FUNCTIONS.f90:171 |
| XP-338 | file-scope mtu12 in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:20505; src/FUNCTIONS.f90:171 |
| XP-339 | file-scope othpl in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:20741; src/FUNCTIONS.f90:171 |
| XP-340 | file-scope pbdv in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:20855; src/FUNCTIONS.f90:171 |
| XP-341 | file-scope pbvv in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:21049; src/FUNCTIONS.f90:171 |
| XP-342 | file-scope pbwa in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:21262; src/FUNCTIONS.f90:171 |
| XP-343 | file-scope psi in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:21432; src/FUNCTIONS.f90:171 |
| XP-344 | file-scope qstar in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:21550; src/FUNCTIONS.f90:171 |
| XP-345 | file-scope rctj in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:21648; src/FUNCTIONS.f90:171 |
| XP-346 | file-scope rcty in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:21763; src/FUNCTIONS.f90:171 |
| XP-347 | file-scope refine in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:21850; src/FUNCTIONS.f90:171 |
| XP-348 | file-scope rmn1 in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:21965; src/FUNCTIONS.f90:171 |
| XP-349 | file-scope rmn2l in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:22187; src/FUNCTIONS.f90:171 |
| XP-350 | file-scope rmn2so in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:22374; src/FUNCTIONS.f90:171 |
| XP-351 | file-scope rmn2sp in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:22514; src/FUNCTIONS.f90:171 |
| XP-352 | file-scope rswfo in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:22753; src/FUNCTIONS.f90:171 |
| XP-353 | file-scope rswfp in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:22848; src/FUNCTIONS.f90:171 |
| XP-354 | file-scope scka in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:22940; src/FUNCTIONS.f90:171 |
| XP-355 | file-scope sckb in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:23124; src/FUNCTIONS.f90:171 |
| XP-356 | file-scope sdmn in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:23244; src/FUNCTIONS.f90:171 |
| XP-357 | file-scope segv in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:23478; src/FUNCTIONS.f90:171 |
| XP-358 | file-scope sphi in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:23694; src/FUNCTIONS.f90:171 |
| XP-359 | file-scope sphj in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:23799; src/FUNCTIONS.f90:171 |
| XP-360 | file-scope sphk in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:23914; src/FUNCTIONS.f90:171 |
| XP-361 | file-scope sphy in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:24004; src/FUNCTIONS.f90:171 |
| XP-362 | file-scope stvh0 in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:24087; src/FUNCTIONS.f90:171 |
| XP-363 | file-scope stvh1 in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:24199; src/FUNCTIONS.f90:171 |
| XP-364 | file-scope stvhv in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:24312; src/FUNCTIONS.f90:171 |
| XP-365 | file-scope stvl0 in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:24497; src/FUNCTIONS.f90:171 |
| XP-366 | file-scope stvl1 in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:24596; src/FUNCTIONS.f90:171 |
| XP-367 | file-scope stvlv in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:24691; src/FUNCTIONS.f90:171 |
| XP-368 | file-scope vvla in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:24849; src/FUNCTIONS.f90:171 |
| XP-369 | file-scope vvsa in functions_special_funcs.f90 (compiled with FUNCTIONS.f90; Zhang/Jin corpus internals not individually traced) | DEP-001, DEP-009 | E3 src/functions_special_funcs.f90:24929; src/FUNCTIONS.f90:171 |
| XP-370 | program linsp (linspace.f90); PARSE_CMD; COMMON_VARS; TOOLS::linspace | DEP-001 | E3 numutils/src/linspace.f90:1-4,12-26 |
| XP-371 | program logsp; PARSE_CMD; COMMON_VARS; TOOLS::logspace | DEP-001 | E3 numutils/src/logspace.f90:1-4,31-48 |
| XP-372 | program arange_; PARSE_CMD; COMMON_VARS; TOOLS::arange | DEP-001 | E3 numutils/src/arange.f90:1-4,9-21 |
| XP-373 | program fermi_; PARSE_CMD; COMMON_VARS; D_UNORDERED_LIST; TOOLS; FUNCTIONS::fermi | DEP-001 | E3 numutils/src/fermi.f90:1-6,17-30 |
| XP-374 | program deriv_; PARSE_CMD; COMMON_VARS; D_UNORDERED_LIST; TOOLS::deriv (USE SPLINE is unused in the executed body) | DEP-001 | E3 numutils/src/deriv.f90:1-6,34-77 |
| XP-375 | program linsp (wmatsubara.f90); PARSE_CMD; COMMON_VARS; TOOLS::arange | DEP-001 | E3 numutils/src/wmatsubara.f90:1-4,10-24 |
| XP-376 | program kdensity; STATISTICS; D_ORDERED_LIST; PARSE_CMD; COMMON_VARS; TOOLS | DEP-001 | E3 numutils/src/kdensity.f90:1-6,25-80 |
| XP-377 | program histogram_; STATISTICS; D_ORDERED_LIST; PARSE_CMD; COMMON_VARS | DEP-001 | E3 numutils/src/histogram.f90:1-5,21-35 |
| XP-378 | program numstat; STATISTICS; D_UNORDERED_LIST; PARSE_CMD; COMMON_VARS | DEP-001 | E3 numutils/src/numstat.f90:1-5,34-57 |
| XP-379 | program plot_3d (splot CLI); SLPLOT::splot3d; PARSE_CMD; COMMON_VARS; D_UNORDERED_LIST; TOOLS | DEP-001, DEP-016, DEP-017 | E3 numutils/src/splot.f90:1-6,23-43,143-151 |
| XP-380 | program spline_; SPLINE::cubic_spline/linear_spline/poly_spline (method switch); PARSE_CMD; COMMON_VARS; D_UNORDERED_LIST; TOOLS | DEP-001, DEP-006, DEP-011, DEP-012 | E3 numutils/src/spline.f90:1-6,18-40,80-87 |
| XP-381 | program pade_; PADE::pade_analytic_continuation; SLREAD::sread; SLPLOT::splot (file write); PARSE_CMD; COMMON_VARS | DEP-001 | E3 numutils/src/pade.f90:1-6,23-39,59-64 |
| XP-382 | program fftgf_; FFTGF (default NR backend); PARSE_CMD; COMMON_VARS; Z_UNORDERED_LIST; TOOLS | DEP-001, DEP-006, DEP-011, DEP-012 | E3 numutils/src/fftgf.f90:1-6,23-49; src/FFTGF_NR.f90:1-11 |
| XP-383 | program random_; PARSE_CMD; COMMON_VARS; RANDOM::init_random_number; Fortran random_number | DEP-001 | E3 numutils/src/random.f90:1-4,14-30,48-67 |
| XP-384 | program func; PARSE_CMD; COMMON_VARS; D_UNORDERED_LIST; evaluator_create_/evaluator_evaluate_x_; -lmatheval | DEP-001, DEP-014 | E3 numutils/src/func.f90:1-4,11-15,58-60; numutils/src/Makefile:44-46 |
| XP-385 | program ffcmplx; IOTOOLS::sread/splot; TOOLS; COMMON_VARS; PARSE_CMD (via help); SYSTEM rm | DEP-001, DEP-016 | E3 numutils/src/ffcmplx.f90:1-4,15-32,41-55 |
| XP-386 | scripts/build.sh; bin/sciforvars.sh; src/Makefile (allmod → libscifor.a); ln FFTGF_NR.f90; gfortran; make/rsync/ar/ranlib; compiles MATRIX mkl_lapack.fi, IOFILE gzip/SYSTEM, SLPLOT gnuplot scripts, FUNCTIONS zerf/dcerf | DEP-001, DEP-003, DEP-004, DEP-006, DEP-007, DEP-008, DEP-009, DEP-010, DEP-011, DEP-012, DEP-013, DEP-015, DEP-016, DEP-017, DEP-018, DEP-020, DEP-021, DEP-022, DEP-029 | E3 scripts/build.sh:1,7,16-35,50-97; src/Makefile:5-12,24-25,149-153; src/MATRIX.f90:10; src/FUNCTIONS.f90:301-302 |
| XP-387 | scripts/fidelity.sh; python3 numeric compare; invokes prebuilt scifor-fidelity | DEP-019, DEP-021 | E3 scripts/fidelity.sh:1,32-68,161-184 |
| XP-388 | program scifor_fidelity; TOOLS::linspace, logspace, deriv; FUNCTIONS::fermi; linked -lscifor -lopenblas -lgfortran | DEP-001, DEP-003 | E3 fidelity/driver.f90:1-66; scripts/build.sh:72-76 |
| XP-389 | bin/sciforvars.sh; sources etc/library.conf (FC=ifort, BLAS/LAPACK paths, MPICH2 paths); optional SFFFTW3/MKLROOT env | DEP-002, DEP-003, DEP-021, DEP-023 | E3 bin/sciforvars.sh:1-38; etc/library.conf:1-8 |
| XP-390 | (appendix) program vfplot; DLPLOT; D_UNORDERED_LIST; COMMON_VARS; TOOLS; VECTORS (DLPLOT/DISLIN not in this tree) | DEP-001, DEP-025 | E3 numutils/src/vfplot.f90:1-6; include/sfmake.inc:57-65 |
| XP-391 | (appendix) no source in checkout; Makefile compiles dfftpack.o + diagPAM.f90 | DEP-024 | E3 numutils/src/Makefile:20-22 |
| XP-392 | (appendix) bin/setup_sf.sh interactive installer; chooses gfortran/ifort; optional MKL; commented FGSL | DEP-001, DEP-002, DEP-004, DEP-021 | E3 bin/setup_sf.sh:1,33-67,178-197,223-228 |
| XP-393 | (appendix) file-scope CHRPACK.f90 (not in src/Makefile allmod; not compiled into libscifor.a) | (none recovered) | E3 src/CHRPACK.f90:1; src/Makefile:5 |
| XP-394 | (appendix) M_UNIINV::uniinv in src/uniinv.f90 (not in allmod) | DEP-010 | E3 src/uniinv.f90:10-22; src/Makefile:5 |
| XP-395 | (appendix) M_UNISTA::unista in src/unista.f90; USE M_UNIINV (not in allmod) | DEP-010 | E3 src/unista.f90:4-13; src/Makefile:5 |
| XP-396 | (appendix) file-scope src/random_routines.f90 (RANDOM.f90 comments out the include) | DEP-028 | E3 src/RANDOM.f90:291; src/random_routines.f90:6-11 |
| XP-397 | (appendix) FFTGF::cfft_1d_ex on MKL/FFTW3 backends only; array sign-flip helper (no DFTI/FFTW call in the ex body) | DEP-001 | E3 src/FFTGF_MKL.f90:11,66-75; src/FFTGF_FFTW3.f90:5,57-66 |
| XP-398 | (appendix) file-scope integrate_d_quadpack.f90 qag/qags/qagi/qagp/qawc/qawf/qawo/qaws and helpers (compiled into libscifor.a) | DEP-001, DEP-008 | E3 src/Makefile:149-153; src/integrate_d_quadpack.f90:1-40,193 |
| XP-399 | (appendix) SPLINE public :: interp_gtau with no matching body (no recovered callee) | (none recovered) | E3 src/SPLINE.f90:38 |
| XP-400 | (appendix) module FFTGF_FFTPACK; zffti/zfftf/zfftb; include splinefft.f90 on tau2iw | DEP-001, DEP-012, DEP-024 | E3 src/FFTGF_FFTPACK.f90:1-21,219; scripts/build.sh:50-62 |

## 2. Reverse index: dependency to paths

| DEP ID | Used by XP IDs | Evidence |
|--------|----------------|----------|
| DEP-001 | XP-001–XP-386, XP-388, XP-390, XP-392, XP-397–XP-398, XP-400 | E3 Fortran sources compiled/linked with gfortran on T1 (`scripts/build.sh:85`; library/CLI/driver units listed in §1) |
| DEP-002 | XP-389, XP-392 | E3 `etc/library.conf:1`; `bin/sciforvars.sh:22`; `bin/setup_sf.sh:33-40` |
| DEP-003 | XP-169–XP-177, XP-386, XP-388–XP-389 | E3 `src/MATRIX.f90:99-347`; `scripts/build.sh:33-35,74-76`; `include/sfmake.inc:16-21` |
| DEP-004 | XP-169–XP-177, XP-386, XP-392 | E3 `src/MATRIX.f90:10`; `src/mkl_lapack.fi:1-18`; `scripts/build.sh:65-70`; `bin/setup_sf.sh:49-67` (MKL FFT backend is optional, not the checkout default) |
| DEP-005 | (none recovered) | E3 no active default-backend path calls FFTW; optional `src/FFTGF_FFTW3.f90:3,20-22`; `scripts/build.sh:50-62` (unused by §1 default edges) |
| DEP-006 | XP-087–XP-089, XP-092–XP-093, XP-096–XP-101, XP-103–XP-105, XP-119, XP-126, XP-175, XP-177–XP-178, XP-183–XP-184, XP-380, XP-382, XP-386 | E3 `src/FFTGF_NR.f90:11,424-427`; `src/BROYDEN.f90:1-6`; `src/BRENT.f90:29-46`; `src/spline_nr_mod.f90:1-14`; `src/INTEGRATE.f90:479-537`; `src/RANDOM.f90:145-176`; `src/MATRIX.f90:58-61` |
| DEP-007 | XP-090–XP-091, XP-386 | E3 `src/WRAP_MINPACK.f90:1,20`; `src/minpack.f90:1383` |
| DEP-008 | XP-119, XP-386, XP-398 | E3 `src/INTEGRATE.f90:345`; `src/integrate_d_quadpack.f90:193,2497`; `src/Makefile:149-153` |
| DEP-009 | XP-205–XP-369, XP-386 | E3 `src/FUNCTIONS.f90:171`; `src/functions_special_funcs.f90:1-29` |
| DEP-010 | XP-068, XP-085–XP-086, XP-386, XP-394–XP-395 | E3 `src/TOOLS.f90:57-69`; `src/uniinv.f90:10-22`; `src/unista.f90:4-13` |
| DEP-011 | XP-100, XP-180, XP-380, XP-382, XP-386 | E3 `src/SPLINE.f90:3,67`; `src/spline_interp.f90:27-37` |
| DEP-012 | XP-099, XP-101, XP-179, XP-380, XP-382, XP-386, XP-400 | E3 `src/SPLINE.f90:4,178`; `src/spline_cubspl_routines.f90:1-8`; `src/splinefft.f90:24` |
| DEP-013 | XP-110, XP-386 | E3 `src/FUNCTIONS.f90:188,281-293`; `src/functions_wofz.f90:1-18` |
| DEP-014 | XP-384 | E3 `numutils/src/func.f90:11-15,58-60`; `numutils/src/Makefile:44-46` |
| DEP-015 | XP-039, XP-041–XP-042, XP-055, XP-386 | E3 `src/IOFILE.f90:219-230,249-266` |
| DEP-016 | XP-041–XP-042, XP-048–XP-049, XP-054–XP-055, XP-075, XP-083, XP-130, XP-379, XP-385–XP-386 | E3 `src/IOFILE.f90:230,266,290`; `src/SQUARE_LATTICE.f90:159-160`; `src/TOOLS.f90:273-274`; `src/slplot_splot_3d.f90:66`; `numutils/src/ffcmplx.f90:52` |
| DEP-017 | XP-054, XP-379, XP-386 | E3 `src/slplot_splot_3d.f90:39-66`; `numutils/src/splot.f90:33-40,143-151` |
| DEP-018 | XP-001, XP-386 | E3 `src/Makefile:2,24-25`; `src/COMVARS.f90:9,112-116` |
| DEP-019 | XP-387 | E3 `scripts/fidelity.sh:32-68` |
| DEP-020 | XP-386 | E3 `src/Makefile:8-12`; `scripts/build.sh:80-81,61` |
| DEP-021 | XP-386–XP-387, XP-389, XP-392 | E3 `scripts/build.sh:1`; `scripts/fidelity.sh:1`; `bin/sciforvars.sh:1`; `bin/setup_sf.sh:1` |
| DEP-022 | XP-386 | E3 `scripts/build.sh:16-24,31-35,83-84` |
| DEP-023 | XP-389 | E3 `etc/library.conf:7-8`; `bin/sciforvars.sh:22` (not linked by `src/Makefile`) |
| DEP-024 | XP-391, XP-400 | E3 `src/FFTGF_FFTPACK.f90:15-21`; `numutils/src/Makefile:20-22` |
| DEP-025 | XP-390 | E3 `numutils/src/vfplot.f90:1-5`; `include/sfmake.inc:57-65` |
| DEP-026 | (none recovered) | E3 commented only: `include/sfmake.inc:42-47`; `bin/sciforvars.sh:58-63` (no recovered path edge) |
| DEP-027 | (none recovered) | E3 commented only: `include/sfmake.inc:50-55`; `bin/setup_sf.sh:223-228`; `bin/sciforvars.sh:65-78` (no recovered path edge) |
| DEP-028 | XP-396 | E3 `src/RANDOM.f90:291`; `src/random_routines.f90:6-11` |
| DEP-029 | XP-111, XP-386 | E3 `src/functions_zerf.f90:1-20`; `src/FUNCTIONS.f90:189,301-302` |

## 4. Open graph questions

- [ ] XP-386 optionally selects `FFT_BACKEND=FFTW3` or `MKL` (`scripts/build.sh:50-62`). Default-checkout edges use NR (DEP-006). FFTW (DEP-005) and MKL-DFT (DEP-004 FFT path) have no active default-backend path row; appendix XP-397 covers `cfft_1d_ex` only.
- [ ] XP-391 (`diagPAM`) and XP-399 (`SPLINE::interp_gtau`) have missing sources/bodies; callees were not invented.
- [ ] Zhang/Jin file-scope paths (XP-205–XP-369) list the compilation unit and DEP-009 only; intra-corpus CALL graphs were not expanded per procedure.
- [ ] COMMON_VARS MPI stubs (`MPIID=0`) are not a linked MPICH2 use; DEP-023 is attached only to jobs that source `etc/library.conf`.
- [ ] Dependency inventory v2 text still says the INTEGRATE public API does not call `qag*`. XP-119 `kramers_kronig` calls `QAWCE` (`src/INTEGRATE.f90:345` → `src/integrate_d_quadpack.f90:2497`). That is DEP-008, not a new ID. Inventory owner may Errata the comment.

## Errata

None.

## Links

- Path inventory: [`docs/modernization/execution-path-inventory.md`](execution-path-inventory.md)
- Dependency inventory: [`docs/modernization/dependency-inventory.md`](dependency-inventory.md)
- Migration plan: [`docs/modernization/migration-plan.md`](migration-plan.md)
