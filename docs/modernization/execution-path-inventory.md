# Execution Path Inventory

**Legacy repo:** `/Users/philipteitel/code/ADD-migrations/sci-fortran-legacy`
**Version:** v1
**Status:** Snapshot
**Date:** 2026-08-25
**Owner:** Archaeologist (`/inventory-paths`)

## Summary

This inventory lists every triggerable path recovered from sci-fortran-legacy (checkout e586903; `src/FFTGF.f90` → `FFTGF_NR.f90`): 389 active paths in §1 and 11 inactive/unknown paths in the appendix. There are no service endpoints, scheduled tasks, or event handlers. Dominant triggers are Fortran library APIs — explicit `public ::` procedures on modules compiled into `libscifor.a` (including modules USE'd by `SCIFOR` and siblings in `src/Makefile` `allmod`) plus 165 file-scope Zhang/Jin special functions compiled by `include "functions_special_funcs.f90"` before `MODULE FUNCTIONS`. Sixteen numutils CLIs, the `scifor_fidelity` driver, and three shell jobs (`build.sh`, `fidelity.sh`, `sciforvars.sh`) are also active. Unknown triggers are `diagPAM` (Makefile target, no source), `FFTGF::cfft_1d_ex` (backend-conditional), and `SPLINE::interp_gtau` (public name with no body). Look next at the dependency graph for what each path pulls in, and at the assessment for the port verdict.

---

## 1. Active paths

| ID | Trigger | Description | Evidence |
|----|---------|-------------|----------|
| XP-001 | library API COMMON_VARS::version | Prints SciFor and caller git revisions and writes version.inc. | E3 src/COMVARS.f90:92,112-124 |
| XP-002 | library API COMMON_VARS::timestamp | Prints a YMDHMS timestamp to stdout or an optional unit. | E3 src/COMVARS.f90:93,136-145 |
| XP-003 | library API COMMON_VARS::abort | Generic alias of error; stops after printing a message. | E3 src/COMVARS.f90:86-88,94 |
| XP-004 | library API COMMON_VARS::error | Prints an error message and stops. | E3 src/COMVARS.f90:94,192-209 |
| XP-005 | library API COMMON_VARS::warning | Prints a warning message and continues. | E3 src/COMVARS.f90:94,211-227 |
| XP-006 | library API COMMON_VARS::msg | Prints a message (MPI-rank gated). | E3 src/COMVARS.f90:95,235-254 |
| XP-007 | library API COMMON_VARS::txtfy | Converts integer/real/complex values to character strings. | E3 src/COMVARS.f90:82-84,96 |
| XP-008 | library API COMMON_VARS::bold | Wraps text in ANSI bold. | E3 src/COMVARS.f90:97 |
| XP-009 | library API COMMON_VARS::underline | Wraps text in ANSI underline. | E3 src/COMVARS.f90:98 |
| XP-010 | library API COMMON_VARS::highlight | Wraps text in ANSI highlight. | E3 src/COMVARS.f90:99 |
| XP-011 | library API COMMON_VARS::erased | Wraps text in ANSI erase/reset styling. | E3 src/COMVARS.f90:100 |
| XP-012 | library API COMMON_VARS::red | Wraps text in ANSI red styling. | E3 src/COMVARS.f90:101 |
| XP-013 | library API COMMON_VARS::green | Wraps text in ANSI green styling. | E3 src/COMVARS.f90:101 |
| XP-014 | library API COMMON_VARS::yellow | Wraps text in ANSI yellow styling. | E3 src/COMVARS.f90:101 |
| XP-015 | library API COMMON_VARS::blue | Wraps text in ANSI blue styling. | E3 src/COMVARS.f90:101 |
| XP-016 | library API COMMON_VARS::purple | Wraps text in ANSI purple styling. | E3 src/COMVARS.f90:101 |
| XP-017 | library API COMMON_VARS::cyan | Wraps text in ANSI cyan styling. | E3 src/COMVARS.f90:101 |
| XP-018 | library API COMMON_VARS::bold_red | Wraps text in ANSI bold red styling. | E3 src/COMVARS.f90:102 |
| XP-019 | library API COMMON_VARS::bold_green | Wraps text in ANSI bold green styling. | E3 src/COMVARS.f90:102 |
| XP-020 | library API COMMON_VARS::bold_yellow | Wraps text in ANSI bold yellow styling. | E3 src/COMVARS.f90:102 |
| XP-021 | library API COMMON_VARS::bold_blue | Wraps text in ANSI bold blue styling. | E3 src/COMVARS.f90:102 |
| XP-022 | library API COMMON_VARS::bold_purple | Wraps text in ANSI bold purple styling. | E3 src/COMVARS.f90:102 |
| XP-023 | library API COMMON_VARS::bold_cyan | Wraps text in ANSI bold cyan styling. | E3 src/COMVARS.f90:102 |
| XP-024 | library API COMMON_VARS::bg_red | Wraps text in ANSI bg red styling. | E3 src/COMVARS.f90:103 |
| XP-025 | library API COMMON_VARS::bg_green | Wraps text in ANSI bg green styling. | E3 src/COMVARS.f90:103 |
| XP-026 | library API COMMON_VARS::bg_yellow | Wraps text in ANSI bg yellow styling. | E3 src/COMVARS.f90:103 |
| XP-027 | library API COMMON_VARS::bg_blue | Wraps text in ANSI bg blue styling. | E3 src/COMVARS.f90:103 |
| XP-028 | library API COMMON_VARS::bg_purple | Wraps text in ANSI bg purple styling. | E3 src/COMVARS.f90:103 |
| XP-029 | library API COMMON_VARS::bg_cyan | Wraps text in ANSI bg cyan styling. | E3 src/COMVARS.f90:103 |
| XP-030 | library API PARSE_CMD::parse_cmd_variable | Parses a named command-line assignment into an integer/real/string/logical variable. | E3 src/PARSECMD.f90:18-23,109-128 |
| XP-031 | library API PARSE_CMD::get_cmd_variable | Returns the i-th command-line token as a name=value cmd_variable. | E3 src/PARSECMD.f90:24,87-100 |
| XP-032 | library API PARSE_CMD::parse_cmd_help | Prints a help buffer and stops if a help flag is present on the command line. | E3 src/PARSECMD.f90:25,36-61 |
| XP-033 | library API PARSE_CMD::print_cmd_help | Prints a help buffer and stops. | E3 src/PARSECMD.f90:26,70-78 |
| XP-034 | library API TIMER::print_bar | Prints a progress bar for iteration i of imax. | E3 src/TIMER.f90:35; src/timer_bar.f90:6 |
| XP-035 | library API TIMER::start_timer | Records a start timestamp for elapsed-time measurement. | E3 src/TIMER.f90:36; src/timer_chrono.f90:5 |
| XP-036 | library API TIMER::stop_timer | Stops the timer and prints elapsed time. | E3 src/TIMER.f90:36; src/timer_chrono.f90:31 |
| XP-037 | library API TIMER::eta | Prints an estimated time of arrival for loop index i of L. | E3 src/TIMER.f90:37; src/timer_chrono.f90:126 |
| XP-038 | library API IOFILE::file_size | Returns the size of a named file via inquire/stat. | E3 src/IOFILE.f90:31,102-118 |
| XP-039 | library API IOFILE::file_length | Counts non-blank lines in a named file. | E3 src/IOFILE.f90:32,174-203 |
| XP-040 | library API IOFILE::file_info | Prints existence/size/length information for a named file. | E3 src/IOFILE.f90:33,131-161 |
| XP-041 | library API IOFILE::data_open | Opens a stored/tarred data file for reading. | E3 src/IOFILE.f90:34,249-268 |
| XP-042 | library API IOFILE::data_store | Stores a data file, optionally compressing when over store_size. | E3 src/IOFILE.f90:35,219-232 |
| XP-043 | library API IOFILE::set_store_size | Sets the kilobyte threshold used by data_store. | E3 src/IOFILE.f90:36,234-238 |
| XP-044 | library API IOFILE::reg_filename | Trims and adjusts a filename string. | E3 src/IOFILE.f90:37,49-53 |
| XP-045 | library API IOFILE::reg | Generic alias of reg_filename. | E3 src/IOFILE.f90:15-17,37 |
| XP-046 | library API IOFILE::txtfit | Generic alias of reg_filename. | E3 src/IOFILE.f90:19-21,37 |
| XP-047 | library API IOFILE::txtcut | Generic alias of reg_filename. | E3 src/IOFILE.f90:23-25,37 |
| XP-048 | library API IOFILE::create_data_dir | Creates a directory (shell mkdir) on a given MPI rank. | E3 src/IOFILE.f90:38,281-292 |
| XP-049 | library API IOFILE::create_dir | Generic alias of create_data_dir. | E3 src/IOFILE.f90:27-29,38 |
| XP-050 | library API IOFILE::close_file | Closes a file after a data_store-style open. | E3 src/IOFILE.f90:39,77 |
| XP-051 | library API IOFILE::get_filename | Returns the basename of a path string. | E3 src/IOFILE.f90:40,55-64 |
| XP-052 | library API IOFILE::get_filepath | Returns the directory prefix of a path string. | E3 src/IOFILE.f90:41,66-75 |
| XP-053 | library API SLPLOT::splot | Writes 1D/2D/3D arrays to a gnuplot-oriented data file (generic over integer/real/complex). | E3 src/SLPLOT.f90:12-25,41 |
| XP-054 | library API SLPLOT::splot3d | Writes a 3D surface (and optional animation) data file. | E3 src/SLPLOT.f90:28-31,42 |
| XP-055 | library API SLPLOT::store_data | Stores array data via the data_save overloads. | E3 src/SLPLOT.f90:34-39,43 |
| XP-056 | library API SLREAD::sread | Reads plotted/saved arrays back from a file (generic over ranks/types). | E3 src/SLREAD.f90:13-29,41 |
| XP-057 | library API SLREAD::read_data | Reads stored array data via the data_read overloads. | E3 src/SLREAD.f90:31-39,42 |
| XP-058 | library API TOOLS::start_loop | Prints a named loop banner and starts the timer. | E3 src/TOOLS.f90:14,121-140 |
| XP-059 | library API TOOLS::end_loop | Stops the timer and prints a loop-end banner. | E3 src/TOOLS.f90:15,147-158 |
| XP-060 | library API TOOLS::linspace | Returns num evenly spaced reals from start to stop, with optional endpoint flags. | E3 src/TOOLS.f90:18; src/tools_grids.f90:1-29 |
| XP-061 | library API TOOLS::logspace | Returns num logarithmically spaced reals from start to stop (default base 10). | E3 src/TOOLS.f90:19; src/tools_grids.f90:32-47 |
| XP-062 | library API TOOLS::arange | Returns num consecutive integers beginning at start. | E3 src/TOOLS.f90:20; src/tools_grids.f90:51-63 |
| XP-063 | library API TOOLS::powspace | Returns a power-law spaced real grid. | E3 src/TOOLS.f90:21; src/tools_grids.f90:147 |
| XP-064 | library API TOOLS::upmspace | Returns a uniform-power-mesh real grid. | E3 src/TOOLS.f90:22; src/tools_grids.f90:96-147 |
| XP-065 | library API TOOLS::upminterval | Builds a two-sided uniform-power mesh about a midpoint. | E3 src/TOOLS.f90:23; src/tools_grids.f90:68-92 |
| XP-066 | library API TOOLS::sort | Sorts a 1D array in place. | E3 src/TOOLS.f90:26; src/tools_sort1d.f90:70 |
| XP-067 | library API TOOLS::sort_array | Returns an order permutation for a 1D array. | E3 src/TOOLS.f90:26; src/tools_sort1d.f90:94 |
| XP-068 | library API TOOLS::uniq | Removes duplicate entries from an integer or real array. | E3 src/TOOLS.f90:27,71-73; src/tools_sort1d.f90:8-24 |
| XP-069 | library API TOOLS::reshuffle | Reorders an array according to an index permutation. | E3 src/TOOLS.f90:27; src/tools_sort1d.f90:51 |
| XP-070 | library API TOOLS::shiftFW | Shifts a matrix or array forward by a step (real/complex overloads). | E3 src/TOOLS.f90:28,49-51; src/tools_shifts.f90:7-43 |
| XP-071 | library API TOOLS::shiftBW | Shifts a matrix or array backward by a step (real/complex overloads). | E3 src/TOOLS.f90:28,53-55; src/tools_shifts.f90:72-108 |
| XP-072 | library API TOOLS::deriv | Returns a finite-difference derivative of a 1D real array at spacing dh. | E3 src/TOOLS.f90:31,175-186 |
| XP-073 | library API TOOLS::gfbethe | Hilbert transform of zeta with Bethe DOS (sign from frequency w). | E3 src/TOOLS.f90:34; src/tools_bethe.f90:65-75 |
| XP-074 | library API TOOLS::gfbether | Hilbert transform of zeta with Bethe DOS (sign from Re zeta). | E3 src/TOOLS.f90:35; src/tools_bethe.f90:86-94 |
| XP-075 | library API TOOLS::bethe_lattice | Builds a Bethe-lattice DOS and omega grid and writes LATTICEinfo/DOSbethe.lattice. | E3 src/TOOLS.f90:36; src/tools_bethe.f90:7-28 |
| XP-076 | library API TOOLS::dens_bethe | Non-interacting Bethe-lattice density of states at energy x. | E3 src/TOOLS.f90:37; src/tools_bethe.f90:41-51 |
| XP-077 | library API TOOLS::dens_hyperc | Non-interacting hypercubic-lattice density of states at energy x. | E3 src/TOOLS.f90:38,193-202 |
| XP-078 | library API TOOLS::find2Dmesh | Locates a 2D point on discrete X/Y grids. | E3 src/TOOLS.f90:41,328 |
| XP-079 | library API TOOLS::get_matsubara_gf_from_dos | Builds a Matsubara Green's function from a spectral density. | E3 src/TOOLS.f90:44,285-312 |
| XP-080 | library API TOOLS::check_convergence | Tests convergence of a 1D array iterate (integer/real/complex, rank 0-2). | E3 src/TOOLS.f90:45,88-99 |
| XP-081 | library API TOOLS::check_convergence_scalar | Tests convergence of a scalar iterate (integer/real/complex, rank 0-2). | E3 src/TOOLS.f90:45,75-86 |
| XP-082 | library API TOOLS::check_convergence_local | Tests local (site-resolved) convergence of a 1D iterate. | E3 src/TOOLS.f90:45,101-112 |
| XP-083 | library API TOOLS::get_free_dos | Accumulates a free DOS from dispersion and k-weights. | E3 src/TOOLS.f90:46,244-276 |
| XP-084 | library API TOOLS::sum_overk_zeta | Sums 1/(zeta-ek) over k-points with weights wk. | E3 src/TOOLS.f90:47,231-238 |
| XP-085 | library API TOOLS::uniinv | Merge-sort inverse ranking of an array with duplicates removed. | E3 src/TOOLS.f90:57-60,417 |
| XP-086 | library API TOOLS::unista | Removes duplicates from an array, keeping first-appearance order. | E3 src/TOOLS.f90:66-69,1181 |
| XP-087 | library API BROYDEN::broydn | Broyden solver for a nonlinear vector function. | E3 src/BROYDEN.f90:6,29 |
| XP-088 | library API BRENT::fzero | Finds a real root of func on interval [a,b] via Brent. | E3 src/BRENT.f90:5,11-20 |
| XP-089 | library API BRENT::zbrent | Brent root finder returning the zero of func on [a,b]. | E3 src/BRENT.f90:6 |
| XP-090 | library API WRAP_MINPACK::fsolve | Solves a nonlinear system in-place via MINPACK hybrd1. | E3 src/WRAP_MINPACK.f90:5,9-22 |
| XP-091 | library API WRAP_MINPACK::ffsolve | Solves a nonlinear system and returns the solution vector via hybrd1. | E3 src/WRAP_MINPACK.f90:5,24-39 |
| XP-092 | library API FFTGF::cfft_1d_forward | In-place forward complex 1D FFT (NR radix-2 four1). | E3 src/FFTGF_NR.f90:6,28-32 |
| XP-093 | library API FFTGF::cfft_1d_backward | In-place backward complex 1D FFT (NR radix-2 four1). | E3 src/FFTGF_NR.f90:6,34-38 |
| XP-094 | library API FFTGF::cfft_1d_shift | Shifts a 2L-length FFT array onto the -L:L convention. | E3 src/FFTGF_NR.f90:6,40-47 |
| XP-095 | library API FFTGF::swap_fftrt2rw | Swaps the two halves of a real-time/frequency FFT array. | E3 src/FFTGF_NR.f90:6,49-59 |
| XP-096 | library API FFTGF::fftgf_rw2rt | FFT of a Green's function from real frequency to real time. | E3 src/FFTGF_NR.f90:7,68-80 |
| XP-097 | library API FFTGF::fftgf_rt2rw | FFT of a Green's function from real time to real frequency. | E3 src/FFTGF_NR.f90:7 |
| XP-098 | library API FFTGF::fftgf_iw2tau | FFT of a Green's function from Matsubara frequency to imaginary time. | E3 src/FFTGF_NR.f90:8 |
| XP-099 | library API FFTGF::fftgf_tau2iw | FFT of a Green's function from imaginary time to Matsubara frequency. | E3 src/FFTGF_NR.f90:8 |
| XP-100 | library API FFTGF::fftff_iw2tau | FFT of a fermionic function from iw to tau. | E3 src/FFTGF_NR.f90:9 |
| XP-101 | library API FFTGF::fftff_tau2iw | FFT of a fermionic function from tau to iw. | E3 src/FFTGF_NR.f90:9 |
| XP-102 | library API FFTGF::fftff_iw2tau_ | Alternate iw-to-tau fermionic FFT entry. | E3 src/FFTGF_NR.f90:10 |
| XP-103 | library API FFTGF::four1 | Numerical Recipes radix-2 complex FFT kernel used by the NR backend. | E3 src/FFTGF_NR.f90:11 |
| XP-104 | library API FFTGF::cosft2 | Numerical Recipes cosine transform used by the NR backend. | E3 src/FFTGF_NR.f90:11 |
| XP-105 | library API FFTGF::realft | Numerical Recipes real-valued FFT used by the NR backend. | E3 src/FFTGF_NR.f90:11 |
| XP-106 | library API FUNCTIONS::heaviside | Elemental Heaviside step (0.5 at x=0). | E3 src/FUNCTIONS.f90:179,198-208 |
| XP-107 | library API FUNCTIONS::step | Pure step function with optional origin-included flag. | E3 src/FUNCTIONS.f90:180,214-227 |
| XP-108 | library API FUNCTIONS::fermi | Elemental Fermi-Dirac occupation 1/(1+exp(beta*x)), saturating for beta*x>100. | E3 src/FUNCTIONS.f90:181,240-248 |
| XP-109 | library API FUNCTIONS::sgn | Sign of an integer or real argument. | E3 src/FUNCTIONS.f90:182-185,260-269 |
| XP-110 | library API FUNCTIONS::wfun | Complex Faddeeva function w(z)=exp(-z^2)erfc(-i z) via wofz. | E3 src/FUNCTIONS.f90:188,284-294 |
| XP-111 | library API FUNCTIONS::zerf | Complex error function erf/erfc (Alan Miller dcerf). | E3 src/FUNCTIONS.f90:189; src/functions_zerf.f90:5-16 |
| XP-112 | library API GREENFUNX::allocate_gf | Allocates matsubara_gf, real_gf, or keldysh_equilibrium_gf containers. | E3 src/GREENFUNX.f90:37-41,63 |
| XP-113 | library API GREENFUNX::deallocate_gf | Deallocates Green's-function containers. | E3 src/GREENFUNX.f90:43-47,64 |
| XP-114 | library API GREENFUNX::assignment(=) | Copies or scalar-fills Green's-function types. | E3 src/GREENFUNX.f90:49-56,65 |
| XP-115 | library API GREENFUNX::operator(+) | Adds two Green's-function containers. | E3 src/GREENFUNX.f90:58-61,66 |
| XP-116 | library API GREENFUNX::ret_component_t | Builds the retarded component from greater/lesser Keldysh components in time. | E3 src/GREENFUNX.f90:67,78-96 |
| XP-117 | library API GREENFUNX::less_component_w | Builds the lesser component in frequency from the retarded function and Fermi function. | E3 src/GREENFUNX.f90:68,101-122 |
| XP-118 | library API GREENFUNX::gtr_component_w | Builds the greater component in frequency from the retarded function and Fermi function. | E3 src/GREENFUNX.f90:69,127-138 |
| XP-119 | library API INTEGRATE::kramers_kronig | Kramers-Kronig transform of a sampled imaginary part. | E3 src/INTEGRATE.f90:62,320 |
| XP-120 | library API INTEGRATE::kronig | Fast Kramers-Kronig integration on a uniform real-frequency mesh. | E3 src/INTEGRATE.f90:63,280-282 |
| XP-121 | library API INTEGRATE::trapz | Trapezoidal integration (real/complex; [a,b], spacing dh, or nonuniform x). | E3 src/INTEGRATE.f90:47-52,64 |
| XP-122 | library API INTEGRATE::simps | Simpson integration (real/complex; [a,b], spacing dh, or nonuniform x). | E3 src/INTEGRATE.f90:54-59,65 |
| XP-123 | library API INTEGRATE::init_finter | Initializes a d_finter or c_finter interpolant object. | E3 src/INTEGRATE.f90:31-33,67 |
| XP-124 | library API INTEGRATE::kill_finter | Destroys a d_finter or c_finter interpolant object. | E3 src/INTEGRATE.f90:35-37,68 |
| XP-125 | library API INTEGRATE::set_finter | Sets interpolant order/range on a d_finter or c_finter object. | E3 src/INTEGRATE.f90:39-41,69 |
| XP-126 | library API INTEGRATE::finter_func | Evaluates a d_finter or c_finter interpolant at x. | E3 src/INTEGRATE.f90:43-45,70 |
| XP-127 | library API SQUARE_LATTICE::square_lattice_dispersion | Returns 2D square-lattice tight-binding epsilon(k) for a vect2D. | E3 src/SQUARE_LATTICE.f90:37,56-68 |
| XP-128 | library API SQUARE_LATTICE::square_lattice_velocity | Returns square-lattice group velocity for a vect2D. | E3 src/SQUARE_LATTICE.f90:38 |
| XP-129 | library API SQUARE_LATTICE::square_lattice_dimension | Sets/returns the k-grid dimension of the square lattice. | E3 src/SQUARE_LATTICE.f90:39 |
| XP-130 | library API SQUARE_LATTICE::square_lattice_structure | Builds the 2D square-lattice k-grid structure. | E3 src/SQUARE_LATTICE.f90:40 |
| XP-131 | library API SQUARE_LATTICE::square_lattice_dispersion_array | Fills an array of square-lattice dispersions on the k-grid. | E3 src/SQUARE_LATTICE.f90:41 |
| XP-132 | library API SQUARE_LATTICE::square_lattice_MGXMpath_dimension | Sets/returns the MGXM path dimension. | E3 src/SQUARE_LATTICE.f90:42 |
| XP-133 | library API SQUARE_LATTICE::square_lattice_MGXMpath_structure | Builds the M-G-X-M path in the 2D Brillouin zone. | E3 src/SQUARE_LATTICE.f90:43 |
| XP-134 | library API SQUARE_LATTICE::square_lattice_reduxGrid_dimension | Sets/returns a reduced k-grid dimension. | E3 src/SQUARE_LATTICE.f90:44 |
| XP-135 | library API SQUARE_LATTICE::square_lattice_reduxGrid_index | Maps reduced-grid indices onto the full k-grid. | E3 src/SQUARE_LATTICE.f90:45 |
| XP-136 | library API SQUARE_LATTICE::square_lattice_reduxGrid_dispersion_array | Fills dispersions on the reduced k-grid. | E3 src/SQUARE_LATTICE.f90:46 |
| XP-137 | library API VECTORS::operator(+) | Adds two vect2D or vect3D values. | E3 src/VECTORS.f90:24-26,54 |
| XP-138 | library API VECTORS::operator(-) | Subtracts two vect2D or vect3D values. | E3 src/VECTORS.f90:27-29,55 |
| XP-139 | library API VECTORS::operator(*) | Scales a vect2D or vect3D by a real. | E3 src/VECTORS.f90:30-32,56 |
| XP-140 | library API VECTORS::operator(.dot.) | Dot product of two vect2D or vect3D values. | E3 src/VECTORS.f90:33-35,57 |
| XP-141 | library API VECTORS::assignment(=) | Assigns vector-to-vector or scalar-to-vector for vect2D/vect3D. | E3 src/VECTORS.f90:36-38,58 |
| XP-142 | library API VECTORS::modulo | Euclidean modulus of a vect2D or vect3D. | E3 src/VECTORS.f90:39-41,59 |
| XP-143 | library API D_ORDERED_LIST::init_list | Allocates an empty ordered real linked list. | E3 src/LIST_D_ORDERED.f90:45 |
| XP-144 | library API D_ORDERED_LIST::destroy_list | Deallocates an ordered real linked list. | E3 src/LIST_D_ORDERED.f90:46 |
| XP-145 | library API D_ORDERED_LIST::insert_element | Inserts a node_object into the ordered real list (alias of insert_element_before). | E3 src/LIST_D_ORDERED.f90:41-43,47 |
| XP-146 | library API D_ORDERED_LIST::insert_element_before | Inserts a node_object before the ordered position. | E3 src/LIST_D_ORDERED.f90:47 |
| XP-147 | library API D_ORDERED_LIST::insert_element_after | Inserts a node_object after the ordered position. | E3 src/LIST_D_ORDERED.f90:47 |
| XP-148 | library API D_ORDERED_LIST::remove_element | Removes an element from the ordered real list. | E3 src/LIST_D_ORDERED.f90:48 |
| XP-149 | library API D_ORDERED_LIST::get_value | Returns the real value stored at a node. | E3 src/LIST_D_ORDERED.f90:49 |
| XP-150 | library API D_ORDERED_LIST::get_node | Returns a node from the ordered real list. | E3 src/LIST_D_ORDERED.f90:50 |
| XP-151 | library API D_ORDERED_LIST::print_list | Prints the ordered real list. | E3 src/LIST_D_ORDERED.f90:51 |
| XP-152 | library API D_ORDERED_LIST::dump_list | Dumps the ordered real list into a rank-1 array. | E3 src/LIST_D_ORDERED.f90:51 |
| XP-153 | library API D_UNORDERED_LIST::init_list | Allocates an empty unordered real linked list. | E3 src/LIST_D_UNORDERED.f90:17,27-33 |
| XP-154 | library API D_UNORDERED_LIST::destroy_list | Deallocates an unordered real linked list. | E3 src/LIST_D_UNORDERED.f90:18,36 |
| XP-155 | library API D_UNORDERED_LIST::add_element | Appends a real value to the unordered list. | E3 src/LIST_D_UNORDERED.f90:19 |
| XP-156 | library API D_UNORDERED_LIST::remove_element | Removes an element from the unordered real list. | E3 src/LIST_D_UNORDERED.f90:20 |
| XP-157 | library API D_UNORDERED_LIST::get_value | Returns a real value from the unordered list. | E3 src/LIST_D_UNORDERED.f90:21 |
| XP-158 | library API D_UNORDERED_LIST::get_node | Returns a node from the unordered real list. | E3 src/LIST_D_UNORDERED.f90:22 |
| XP-159 | library API D_UNORDERED_LIST::print_list | Prints the unordered real list. | E3 src/LIST_D_UNORDERED.f90:23 |
| XP-160 | library API D_UNORDERED_LIST::dump_list | Dumps the unordered real list into a rank-1 array. | E3 src/LIST_D_UNORDERED.f90:23 |
| XP-161 | library API Z_UNORDERED_LIST::init_list | Allocates an empty unordered complex linked list. | E3 src/LIST_Z_UNORDERED.f90:17,27 |
| XP-162 | library API Z_UNORDERED_LIST::destroy_list | Deallocates an unordered complex linked list. | E3 src/LIST_Z_UNORDERED.f90:18 |
| XP-163 | library API Z_UNORDERED_LIST::add_element | Appends a complex value to the unordered list. | E3 src/LIST_Z_UNORDERED.f90:19 |
| XP-164 | library API Z_UNORDERED_LIST::remove_element | Removes an element from the unordered complex list. | E3 src/LIST_Z_UNORDERED.f90:20 |
| XP-165 | library API Z_UNORDERED_LIST::get_value | Returns a complex value from the unordered list. | E3 src/LIST_Z_UNORDERED.f90:21 |
| XP-166 | library API Z_UNORDERED_LIST::get_node | Returns a node from the unordered complex list. | E3 src/LIST_Z_UNORDERED.f90:22 |
| XP-167 | library API Z_UNORDERED_LIST::print_list | Prints the unordered complex list. | E3 src/LIST_Z_UNORDERED.f90:23 |
| XP-168 | library API Z_UNORDERED_LIST::dump_list | Dumps the unordered complex list into a rank-1 array. | E3 src/LIST_Z_UNORDERED.f90:23 |
| XP-169 | library API MATRIX::matrix_diagonalize | Diagonalizes a real or complex matrix (LAPACK). | E3 src/MATRIX.f90:46-48,63 |
| XP-170 | library API MATRIX::solve_linear_system | Solves a linear system (LAPACK). | E3 src/MATRIX.f90:52-55,65 |
| XP-171 | library API MATRIX::matrix_inverse | Inverts a real or complex general matrix. | E3 src/MATRIX.f90:14-16,67 |
| XP-172 | library API MATRIX::matrix_inverse_sym | Inverts a real or complex symmetric matrix. | E3 src/MATRIX.f90:18-20,68 |
| XP-173 | library API MATRIX::matrix_inverse_her | Inverts a complex Hermitian matrix. | E3 src/MATRIX.f90:22-24,69 |
| XP-174 | library API MATRIX::matrix_inverse_triang | Inverts a real or complex triangular matrix. | E3 src/MATRIX.f90:26-28,70 |
| XP-175 | library API MATRIX::matrix_inverse_gj | Inverts a matrix by Gauss-Jordan elimination. | E3 src/MATRIX.f90:30-32,71 |
| XP-176 | library API MATRIX::m_invert | Generic matrix inverse (non-GJ overloads). | E3 src/MATRIX.f90:40-42,73 |
| XP-177 | library API MATRIX::m_invert_gj | Generic Gauss-Jordan matrix inverse. | E3 src/MATRIX.f90:36-38,74 |
| XP-178 | library API SPLINE::poly_spline | Polynomial interpolation of real or complex sampled data onto a new mesh. | E3 src/SPLINE.f90:18-20,34,94 |
| XP-179 | library API SPLINE::cubic_spline | Cubic-spline interpolation of real or complex sampled data onto a new mesh. | E3 src/SPLINE.f90:22-24,35,156 |
| XP-180 | library API SPLINE::linear_spline | Linear interpolation of real or complex sampled data onto a new mesh. | E3 src/SPLINE.f90:26-28,36,47 |
| XP-181 | library API SPLINE::extract | Generic alias of extract_gtau. | E3 src/SPLINE.f90:30-32,37 |
| XP-182 | library API SPLINE::extract_gtau | Downsamples G(tau) from N+1 points onto Nfak+1 points. | E3 src/SPLINE.f90:37,254-279 |
| XP-183 | library API SPLINE_FINTER_MOD::finter | Polynomial interpolant of module-level finterX/finterF samples at x. | E3 src/spline_finter_mod.f90:9,13-32 |
| XP-184 | library API RANDOM::nrand | Park-Miller style seeded uniform real generator. | E3 src/RANDOM.f90:26,145-176 |
| XP-185 | library API RANDOM::irand | Integer random near 0..10 via Fortran random_number. | E3 src/RANDOM.f90:27,34-39 |
| XP-186 | library API RANDOM::drand | Uniform real(8) via Fortran random_number. | E3 src/RANDOM.f90:27,40-43 |
| XP-187 | library API RANDOM::crand | Uniform complex(8) via two random_number draws. | E3 src/RANDOM.f90:27,44-50 |
| XP-188 | library API RANDOM::init_random_number | Seeds the Fortran random_number generator. | E3 src/RANDOM.f90:28 |
| XP-189 | library API RANDOM::random_order | Returns a random permutation of indices. | E3 src/RANDOM.f90:29 |
| XP-190 | library API RANDOM::rand | Fills integer/real/complex scalar, vector, or matrix with scaled random values. | E3 src/RANDOM.f90:20-24,30 |
| XP-191 | library API STATISTICS::histogram_allocate | Allocates a histogram type with n bins. | E3 src/STATISTICS.f90:11 |
| XP-192 | library API STATISTICS::histogram_set_range_uniform | Sets a uniform bin range on a histogram. | E3 src/STATISTICS.f90:12 |
| XP-193 | library API STATISTICS::histogram_accumulate | Adds a weighted sample to a histogram. | E3 src/STATISTICS.f90:13 |
| XP-194 | library API STATISTICS::histogram_get_range | Returns the range of a histogram bin. | E3 src/STATISTICS.f90:14 |
| XP-195 | library API STATISTICS::histogram_get_value | Returns the accumulated value of a histogram bin. | E3 src/STATISTICS.f90:15 |
| XP-196 | library API STATISTICS::histogram_print | Prints histogram bins to a unit. | E3 src/STATISTICS.f90:16 |
| XP-197 | library API STATISTICS::get_moments | Computes mean, sdev, var, skew, and kurtosis of a real sample. | E3 src/STATISTICS.f90:19,27 |
| XP-198 | library API STATISTICS::get_mean | Returns the mean of a real sample. | E3 src/STATISTICS.f90:20 |
| XP-199 | library API STATISTICS::get_sd | Returns the standard deviation of a real sample. | E3 src/STATISTICS.f90:20 |
| XP-200 | library API STATISTICS::get_var | Returns the variance of a real sample. | E3 src/STATISTICS.f90:20 |
| XP-201 | library API STATISTICS::get_skew | Returns the skewness of a real sample. | E3 src/STATISTICS.f90:20 |
| XP-202 | library API STATISTICS::get_curt | Returns the kurtosis of a real sample. | E3 src/STATISTICS.f90:20 |
| XP-203 | library API STATISTICS::get_covariance | Returns the covariance matrix of a multi-column sample. | E3 src/STATISTICS.f90:21 |
| XP-204 | library API PADE::pade_analytic_continuation | Pade analytic continuation from Matsubara data gm(wm) onto points x. | E3 src/PADE.f90:9,14-20 |
| XP-205 | library API file-scope airya | File-scope Zhang/Jin special function: Airy functions and their derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:1; src/FUNCTIONS.f90:171 |
| XP-206 | library API file-scope airyb | File-scope Zhang/Jin special function: Airy functions and their derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:90; src/FUNCTIONS.f90:171 |
| XP-207 | library API file-scope airyzo | File-scope Zhang/Jin special function: the first NT zeros of Ai(x) and Ai'(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:319; src/FUNCTIONS.f90:171 |
| XP-208 | library API file-scope ajyik | File-scope Zhang/Jin special function: Bessel functions Jv(x), Yv(x), Iv(x), Kv(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:496; src/FUNCTIONS.f90:171 |
| XP-209 | library API file-scope aswfa | File-scope Zhang/Jin special function: prolate and oblate spheroidal angular functions of the first kind (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:807; src/FUNCTIONS.f90:171 |
| XP-210 | library API file-scope aswfb | File-scope Zhang/Jin special function: prolate and oblate spheroidal angular functions of the first kind (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:955; src/FUNCTIONS.f90:171 |
| XP-211 | library API file-scope bernoa | File-scope Zhang/Jin special function: the Bernoulli number Bn (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:1062; src/FUNCTIONS.f90:171 |
| XP-212 | library API file-scope bernob | File-scope Zhang/Jin special function: the Bernoulli number Bn (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:1128; src/FUNCTIONS.f90:171 |
| XP-213 | library API file-scope betaf | File-scope Zhang/Jin special function: the Beta function B(p,q) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:1200; src/FUNCTIONS.f90:171 |
| XP-214 | library API file-scope bjndd | File-scope Zhang/Jin special function: Bessel functions Jn(x) and first and second derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:1255; src/FUNCTIONS.f90:171 |
| XP-215 | library API file-scope cbk | File-scope Zhang/Jin special function: coefficients for oblate radial functions with small argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:1346; src/FUNCTIONS.f90:171 |
| XP-216 | library API file-scope cchg | File-scope Zhang/Jin special function: the confluent hypergeometric function (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:1511; src/FUNCTIONS.f90:171 |
| XP-217 | library API file-scope cerf | File-scope Zhang/Jin special function: the error function and derivative for a complex argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:1730; src/FUNCTIONS.f90:171 |
| XP-218 | library API file-scope cerror | File-scope Zhang/Jin special function: the error function for a complex argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:1870; src/FUNCTIONS.f90:171 |
| XP-219 | library API file-scope cerzo | File-scope Zhang/Jin special function: the complex zeros of the error function (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:1963; src/FUNCTIONS.f90:171 |
| XP-220 | library API file-scope cfc | File-scope Zhang/Jin special function: the complex Fresnel integral C(z) and C'(z) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:2075; src/FUNCTIONS.f90:171 |
| XP-221 | library API file-scope cfs | File-scope Zhang/Jin special function: the complex Fresnel integral S(z) and S'(z) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:2199; src/FUNCTIONS.f90:171 |
| XP-222 | library API file-scope cgama | File-scope Zhang/Jin special function: the Gamma function for complex argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:2323; src/FUNCTIONS.f90:171 |
| XP-223 | library API file-scope ch12n | File-scope Zhang/Jin special function: Hankel functions of first and second kinds, complex argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:2467; src/FUNCTIONS.f90:171 |
| XP-224 | library API file-scope chgm | File-scope Zhang/Jin special function: the confluent hypergeometric function M(a,b,x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:2592; src/FUNCTIONS.f90:171 |
| XP-225 | library API file-scope chgu | File-scope Zhang/Jin special function: the confluent hypergeometric function U(a,b,x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:2776; src/FUNCTIONS.f90:171 |
| XP-226 | library API file-scope chgubi | File-scope Zhang/Jin special function: confluent hypergeometric function with integer argument B (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:2901; src/FUNCTIONS.f90:171 |
| XP-227 | library API file-scope chguit | File-scope Zhang/Jin special function: the hypergeometric function using Gauss-Legendre integration (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:3125; src/FUNCTIONS.f90:171 |
| XP-228 | library API file-scope chgul | File-scope Zhang/Jin special function: confluent hypergeometric function U(a,b,x) for large argument X (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:3294; src/FUNCTIONS.f90:171 |
| XP-229 | library API file-scope chgus | File-scope Zhang/Jin special function: confluent hypergeometric function U(a,b,x) for small argument X (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:3394; src/FUNCTIONS.f90:171 |
| XP-230 | library API file-scope cik01 | File-scope Zhang/Jin special function: modified Bessel I0(z), I1(z), K0(z) and K1(z) for complex argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:3493; src/FUNCTIONS.f90:171 |
| XP-231 | library API file-scope ciklv | File-scope Zhang/Jin special function: modified Bessel functions Iv(z), Kv(z), complex argument, large order (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:3706; src/FUNCTIONS.f90:171 |
| XP-232 | library API file-scope cikna | File-scope Zhang/Jin special function: modified Bessel functions In(z), Kn(z), derivatives, complex argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:3822; src/FUNCTIONS.f90:171 |
| XP-233 | library API file-scope ciknb | File-scope Zhang/Jin special function: complex modified Bessel functions In(z) and Kn(z) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:3961; src/FUNCTIONS.f90:171 |
| XP-234 | library API file-scope cikva | File-scope Zhang/Jin special function: modified Bessel functions Iv(z), Kv(z), arbitrary order, complex (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:4165; src/FUNCTIONS.f90:171 |
| XP-235 | library API file-scope cikvb | File-scope Zhang/Jin special function: modified Bessel functions,Iv(z), Kv(z), arbitrary order, complex (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:4462; src/FUNCTIONS.f90:171 |
| XP-236 | library API file-scope cisia | File-scope Zhang/Jin special function: cosine Ci(x) and sine integrals Si(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:4738; src/FUNCTIONS.f90:171 |
| XP-237 | library API file-scope cisib | File-scope Zhang/Jin special function: cosine and sine integrals (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:4889; src/FUNCTIONS.f90:171 |
| XP-238 | library API file-scope cjk | File-scope Zhang/Jin special function: asymptotic expansion coefficients for Bessel functions of large order (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:4984; src/FUNCTIONS.f90:171 |
| XP-239 | library API file-scope cjy01 | File-scope Zhang/Jin special function: complexBessel functions, derivatives, J0(z), J1(z), Y0(z), Y1(z) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:5062; src/FUNCTIONS.f90:171 |
| XP-240 | library API file-scope cjylv | File-scope Zhang/Jin special function: Bessel functions Jv(z), Yv(z) of complex argument and large order v (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:5290; src/FUNCTIONS.f90:171 |
| XP-241 | library API file-scope cjyna | File-scope Zhang/Jin special function: Bessel functions and derivatives, Jn(z) and Yn(z) of complex argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:5402; src/FUNCTIONS.f90:171 |
| XP-242 | library API file-scope cjynb | File-scope Zhang/Jin special function: Bessel functions, derivatives, Jn(z) and Yn(z) of complex argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:5662; src/FUNCTIONS.f90:171 |
| XP-243 | library API file-scope cjyva | File-scope Zhang/Jin special function: Bessel functions and derivatives, Jv(z) and Yv(z) of complex argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:5888; src/FUNCTIONS.f90:171 |
| XP-244 | library API file-scope cjyvb | File-scope Zhang/Jin special function: Bessel functions and derivatives, Jv(z) and Yv(z) of complex argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:6339; src/FUNCTIONS.f90:171 |
| XP-245 | library API file-scope clpmn | File-scope Zhang/Jin special function: associated Legendre functions and derivatives for complex argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:6627; src/FUNCTIONS.f90:171 |
| XP-246 | library API file-scope clpn | File-scope Zhang/Jin special function: Legendre functions and derivatives for complex argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:6758; src/FUNCTIONS.f90:171 |
| XP-247 | library API file-scope clqmn | File-scope Zhang/Jin special function: associated Legendre functions and derivatives for complex argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:6838; src/FUNCTIONS.f90:171 |
| XP-248 | library API file-scope clqn | File-scope Zhang/Jin special function: Legendre function Qn(z) and derivative Wn'(z) for complex argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:7025; src/FUNCTIONS.f90:171 |
| XP-249 | library API file-scope comelp | File-scope Zhang/Jin special function: complete elliptic integrals K(k) and E(k) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:7146; src/FUNCTIONS.f90:171 |
| XP-250 | library API file-scope cpbdn | File-scope Zhang/Jin special function: parabolic cylinder function Dn(z) and Dn'(z) for complex argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:7235; src/FUNCTIONS.f90:171 |
| XP-251 | library API file-scope cpdla | File-scope Zhang/Jin special function: complex parabolic cylinder function Dn(z) for large argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:7394; src/FUNCTIONS.f90:171 |
| XP-252 | library API file-scope cpdsa | File-scope Zhang/Jin special function: complex parabolic cylinder function Dn(z) for small argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:7460; src/FUNCTIONS.f90:171 |
| XP-253 | library API file-scope cpsi | File-scope Zhang/Jin special function: the psi function for a complex argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:7570; src/FUNCTIONS.f90:171 |
| XP-254 | library API file-scope csphik | File-scope Zhang/Jin special function: complex modified spherical Bessel functions and derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:7699; src/FUNCTIONS.f90:171 |
| XP-255 | library API file-scope csphjy | File-scope Zhang/Jin special function: spherical Bessel functions jn(z) and yn(z) for complex argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:7842; src/FUNCTIONS.f90:171 |
| XP-256 | library API file-scope cv0 | File-scope Zhang/Jin special function: the initial characteristic value of Mathieu functions (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:7982; src/FUNCTIONS.f90:171 |
| XP-257 | library API file-scope cva1 | File-scope Zhang/Jin special function: a sequence of characteristic values of Mathieu functions (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:8359; src/FUNCTIONS.f90:171 |
| XP-258 | library API file-scope cva2 | File-scope Zhang/Jin special function: a specific characteristic value of Mathieu functions (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:8571; src/FUNCTIONS.f90:171 |
| XP-259 | library API file-scope cvf | File-scope Zhang/Jin special function: F for the characteristic equation of Mathieu functions (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:8729; src/FUNCTIONS.f90:171 |
| XP-260 | library API file-scope cvql | File-scope Zhang/Jin special function: the characteristic value of Mathieu functions for q <= 3*m (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:8859; src/FUNCTIONS.f90:171 |
| XP-261 | library API file-scope cvqm | File-scope Zhang/Jin special function: the characteristic value of Mathieu functions for q <= m*m (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:8950; src/FUNCTIONS.f90:171 |
| XP-262 | library API file-scope cy01 | File-scope Zhang/Jin special function: complex Bessel functions Y0(z) and Y1(z) and derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:9003; src/FUNCTIONS.f90:171 |
| XP-263 | library API file-scope cyzo | File-scope Zhang/Jin special function: zeros of complex Bessel functions Y0(z) and Y1(z) and Y1'(z) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:9252; src/FUNCTIONS.f90:171 |
| XP-264 | library API file-scope dvla | File-scope Zhang/Jin special function: parabolic cylinder functions Dv(x) for large argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:9408; src/FUNCTIONS.f90:171 |
| XP-265 | library API file-scope dvsa | File-scope Zhang/Jin special function: parabolic cylinder functions Dv(x) for small argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:9485; src/FUNCTIONS.f90:171 |
| XP-266 | library API file-scope e1xa | File-scope Zhang/Jin special function: the exponential integral E1(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:9589; src/FUNCTIONS.f90:171 |
| XP-267 | library API file-scope e1xb | File-scope Zhang/Jin special function: the exponential integral E1(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:9664; src/FUNCTIONS.f90:171 |
| XP-268 | library API file-scope e1z | File-scope Zhang/Jin special function: the complex exponential integral E1(z) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:9743; src/FUNCTIONS.f90:171 |
| XP-269 | library API file-scope eix | File-scope Zhang/Jin special function: the exponential integral Ei(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:9828; src/FUNCTIONS.f90:171 |
| XP-270 | library API file-scope elit | File-scope Zhang/Jin special function: complete and incomplete elliptic integrals F(k,phi) and E(k,phi) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:9903; src/FUNCTIONS.f90:171 |
| XP-271 | library API file-scope elit3 | File-scope Zhang/Jin special function: the elliptic integral of the third kind (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:10012; src/FUNCTIONS.f90:171 |
| XP-272 | library API file-scope envj | File-scope Zhang/Jin special function: a utility function used by MSTA1 and MSTA2 (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:10113; src/FUNCTIONS.f90:171 |
| XP-273 | library API file-scope enxa | File-scope Zhang/Jin special function: the exponential integral En(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:10160; src/FUNCTIONS.f90:171 |
| XP-274 | library API file-scope enxb | File-scope Zhang/Jin special function: the exponential integral En(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:10218; src/FUNCTIONS.f90:171 |
| XP-275 | library API file-scope werror | File-scope Zhang/Jin special function: the error function (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:10332; src/FUNCTIONS.f90:171 |
| XP-276 | library API file-scope eulera | File-scope Zhang/Jin special function: the Euler number En (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:10419; src/FUNCTIONS.f90:171 |
| XP-277 | library API file-scope eulerb | File-scope Zhang/Jin special function: the Euler number En (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:10480; src/FUNCTIONS.f90:171 |
| XP-278 | library API file-scope fcoef | File-scope Zhang/Jin special function: expansion coefficients for Mathieu and modified Mathieu functions (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:10551; src/FUNCTIONS.f90:171 |
| XP-279 | library API file-scope fcs | File-scope Zhang/Jin special function: Fresnel integrals C(x) and S(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:10860; src/FUNCTIONS.f90:171 |
| XP-280 | library API file-scope fcszo | File-scope Zhang/Jin special function: complex zeros of Fresnel integrals C(x) or S(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:11011; src/FUNCTIONS.f90:171 |
| XP-281 | library API file-scope ffk | File-scope Zhang/Jin special function: modified Fresnel integrals F+/-(x) and K+/-(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:11148; src/FUNCTIONS.f90:171 |
| XP-282 | library API file-scope gaih | File-scope Zhang/Jin special function: the GammaH function (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:11369; src/FUNCTIONS.f90:171 |
| XP-283 | library API file-scope gam0 | File-scope Zhang/Jin special function: the Gamma function for the LAMV function (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:11430; src/FUNCTIONS.f90:171 |
| XP-284 | library API file-scope gammaf | File-scope Zhang/Jin special function: the Gamma function (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:11507; src/FUNCTIONS.f90:171 |
| XP-285 | library API file-scope gmn | File-scope Zhang/Jin special function: quantities for oblate radial functions with small argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:11627; src/FUNCTIONS.f90:171 |
| XP-286 | library API file-scope herzo | File-scope Zhang/Jin special function: the zeros the Hermite polynomial Hn(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:11738; src/FUNCTIONS.f90:171 |
| XP-287 | library API file-scope hygfx | File-scope Zhang/Jin special function: the hypergeometric function F(A,B,C,X) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:11887; src/FUNCTIONS.f90:171 |
| XP-288 | library API file-scope hygfz | File-scope Zhang/Jin special function: the hypergeometric function F(a,b,c,x) for complex argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:12299; src/FUNCTIONS.f90:171 |
| XP-289 | library API file-scope ik01a | File-scope Zhang/Jin special function: Bessel function I0(x), I1(x), K0(x), and K1(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:12818; src/FUNCTIONS.f90:171 |
| XP-290 | library API file-scope ik01b | File-scope Zhang/Jin special function: Bessel functions I0(x), I1(x), K0(x), and K1(x) and derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:13005; src/FUNCTIONS.f90:171 |
| XP-291 | library API file-scope ikna | File-scope Zhang/Jin special function: Bessel function In(x) and Kn(x), and derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:13172; src/FUNCTIONS.f90:171 |
| XP-292 | library API file-scope iknb | File-scope Zhang/Jin special function: Bessel function In(x) and Kn(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:13325; src/FUNCTIONS.f90:171 |
| XP-293 | library API file-scope ikv | File-scope Zhang/Jin special function: modified Bessel function Iv(x) and Kv(x) and their derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:13496; src/FUNCTIONS.f90:171 |
| XP-294 | library API file-scope incob | File-scope Zhang/Jin special function: the incomplete beta function Ix(a,b) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:13760; src/FUNCTIONS.f90:171 |
| XP-295 | library API file-scope incog | File-scope Zhang/Jin special function: the incomplete gamma function r(a,x), ,(a,x), P(a,x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:13857; src/FUNCTIONS.f90:171 |
| XP-296 | library API file-scope itairy | File-scope Zhang/Jin special function: the integrals of Airy functions (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:13956; src/FUNCTIONS.f90:171 |
| XP-297 | library API file-scope itika | File-scope Zhang/Jin special function: the integral of the modified Bessel functions I0(t) and K0(t) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:14143; src/FUNCTIONS.f90:171 |
| XP-298 | library API file-scope itikb | File-scope Zhang/Jin special function: the integral of the Bessel functions I0(t) and K0(t) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:14282; src/FUNCTIONS.f90:171 |
| XP-299 | library API file-scope itjya | File-scope Zhang/Jin special function: integrals of Bessel functions J0(t) and Y0(t) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:14437; src/FUNCTIONS.f90:171 |
| XP-300 | library API file-scope itjyb | File-scope Zhang/Jin special function: integrals of Bessel functions J0(t) and Y0(t) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:14578; src/FUNCTIONS.f90:171 |
| XP-301 | library API file-scope itsh0 | File-scope Zhang/Jin special function: the Struve function H0(t) from 0 to x (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:14725; src/FUNCTIONS.f90:171 |
| XP-302 | library API file-scope itsl0 | File-scope Zhang/Jin special function: the Struve function L0(t) from 0 to x (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:14860; src/FUNCTIONS.f90:171 |
| XP-303 | library API file-scope itth0 | File-scope Zhang/Jin special function: H0(t)/t from x to oo (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:14979; src/FUNCTIONS.f90:171 |
| XP-304 | library API file-scope ittika | File-scope Zhang/Jin special function: (I0(t)-1)/t from 0 to x, K0(t)/t from x to infinity (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:15080; src/FUNCTIONS.f90:171 |
| XP-305 | library API file-scope ittikb | File-scope Zhang/Jin special function: (I0(t)-1)/t from 0 to x, K0(t)/t from x to infinity (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:15205; src/FUNCTIONS.f90:171 |
| XP-306 | library API file-scope ittjya | File-scope Zhang/Jin special function: (1-J0(t))/t from 0 to x, and Y0(t)/t from x to infinity (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:15338; src/FUNCTIONS.f90:171 |
| XP-307 | library API file-scope ittjyb | File-scope Zhang/Jin special function: (1-J0(t))/t from 0 to x, and Y0(t)/t from x to infinity (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:15509; src/FUNCTIONS.f90:171 |
| XP-308 | library API file-scope jdzo | File-scope Zhang/Jin special function: the zeros of Bessel functions Jn(x) and Jn'(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:15653; src/FUNCTIONS.f90:171 |
| XP-309 | library API file-scope jelp | File-scope Zhang/Jin special function: Jacobian elliptic functions SN(u), CN(u), DN(u) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:15892; src/FUNCTIONS.f90:171 |
| XP-310 | library API file-scope jy01a | File-scope Zhang/Jin special function: Bessel functions J0(x), J1(x), Y0(x), Y1(x) and derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:15987; src/FUNCTIONS.f90:171 |
| XP-311 | library API file-scope jy01b | File-scope Zhang/Jin special function: Bessel functions J0(x), J1(x), Y0(x), Y1(x) and derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:16191; src/FUNCTIONS.f90:171 |
| XP-312 | library API file-scope jyna | File-scope Zhang/Jin special function: Bessel functions Jn(x) and Yn(x) and derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:16368; src/FUNCTIONS.f90:171 |
| XP-313 | library API file-scope jynb | File-scope Zhang/Jin special function: Bessel functions Jn(x) and Yn(x) and derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:16527; src/FUNCTIONS.f90:171 |
| XP-314 | library API file-scope jyndd | File-scope Zhang/Jin special function: Bessel functions Jn(x) and Yn(x), first and second derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:16730; src/FUNCTIONS.f90:171 |
| XP-315 | library API file-scope jyv | File-scope Zhang/Jin special function: Bessel functions Jv(x) and Yv(x) and their derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:16848; src/FUNCTIONS.f90:171 |
| XP-316 | library API file-scope jyzo | File-scope Zhang/Jin special function: the zeros of Bessel functions Jn(x), Yn(x) and derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:17173; src/FUNCTIONS.f90:171 |
| XP-317 | library API file-scope klvna | File-scope Zhang/Jin special function: Kelvin functions ber(x), bei(x), ker(x), and kei(x), and derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:17356; src/FUNCTIONS.f90:171 |
| XP-318 | library API file-scope klvnb | File-scope Zhang/Jin special function: Kelvin functions ber(x), bei(x), ker(x), and kei(x), and derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:17628; src/FUNCTIONS.f90:171 |
| XP-319 | library API file-scope klvnzo | File-scope Zhang/Jin special function: zeros of the Kelvin functions (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:17889; src/FUNCTIONS.f90:171 |
| XP-320 | library API file-scope kmn | File-scope Zhang/Jin special function: expansion coefficients of prolate or oblate spheroidal functions (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:18009; src/FUNCTIONS.f90:171 |
| XP-321 | library API file-scope lagzo | File-scope Zhang/Jin special function: zeros of the Laguerre polynomial, and integration weights (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:18225; src/FUNCTIONS.f90:171 |
| XP-322 | library API file-scope lamn | File-scope Zhang/Jin special function: lambda functions and derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:18351; src/FUNCTIONS.f90:171 |
| XP-323 | library API file-scope lamv | File-scope Zhang/Jin special function: lambda functions and derivatives of arbitrary order (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:18508; src/FUNCTIONS.f90:171 |
| XP-324 | library API file-scope legzo | File-scope Zhang/Jin special function: the zeros of Legendre polynomials, and integration weights (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:18749; src/FUNCTIONS.f90:171 |
| XP-325 | library API file-scope lgama | File-scope Zhang/Jin special function: the gamma function or its logarithm (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:18872; src/FUNCTIONS.f90:171 |
| XP-326 | library API file-scope lpmn | File-scope Zhang/Jin special function: associated Legendre functions Pmn(X) and derivatives P'mn(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:18969; src/FUNCTIONS.f90:171 |
| XP-327 | library API file-scope lpmns | File-scope Zhang/Jin special function: associated Legendre functions Pmn(X) and derivatives P'mn(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:19093; src/FUNCTIONS.f90:171 |
| XP-328 | library API file-scope lpmv | File-scope Zhang/Jin special function: associated Legendre functions Pmv(X) with arbitrary degree (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:19202; src/FUNCTIONS.f90:171 |
| XP-329 | library API file-scope lpn | File-scope Zhang/Jin special function: Legendre polynomials Pn(x) and derivatives Pn'(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:19401; src/FUNCTIONS.f90:171 |
| XP-330 | library API file-scope lpni | File-scope Zhang/Jin special function: Legendre polynomials Pn(x), derivatives, and integrals (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:19476; src/FUNCTIONS.f90:171 |
| XP-331 | library API file-scope lqmn | File-scope Zhang/Jin special function: associated Legendre functions Qmn(x) and derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:19573; src/FUNCTIONS.f90:171 |
| XP-332 | library API file-scope lqmns | File-scope Zhang/Jin special function: associated Legendre functions Qmn(x) and derivatives Qmn'(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:19751; src/FUNCTIONS.f90:171 |
| XP-333 | library API file-scope lqna | File-scope Zhang/Jin special function: Legendre function Qn(x) and derivatives Qn'(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:19975; src/FUNCTIONS.f90:171 |
| XP-334 | library API file-scope lqnb | File-scope Zhang/Jin special function: Legendre function Qn(x) and derivatives Qn'(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:20051; src/FUNCTIONS.f90:171 |
| XP-335 | library API file-scope msta1 | File-scope Zhang/Jin special function: a backward recurrence starting point for Jn(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:20190; src/FUNCTIONS.f90:171 |
| XP-336 | library API file-scope msta2 | File-scope Zhang/Jin special function: a backward recurrence starting point for Jn(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:20270; src/FUNCTIONS.f90:171 |
| XP-337 | library API file-scope mtu0 | File-scope Zhang/Jin special function: Mathieu functions CEM(x,q) and SEM(x,q) and derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:20364; src/FUNCTIONS.f90:171 |
| XP-338 | library API file-scope mtu12 | File-scope Zhang/Jin special function: modified Mathieu functions of the first and second kind (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:20505; src/FUNCTIONS.f90:171 |
| XP-339 | library API file-scope othpl | File-scope Zhang/Jin special function: orthogonal polynomials Tn(x), Un(x), Ln(x) or Hn(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:20741; src/FUNCTIONS.f90:171 |
| XP-340 | library API file-scope pbdv | File-scope Zhang/Jin special function: parabolic cylinder functions Dv(x) and derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:20855; src/FUNCTIONS.f90:171 |
| XP-341 | library API file-scope pbvv | File-scope Zhang/Jin special function: parabolic cylinder functions Vv(x) and their derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:21049; src/FUNCTIONS.f90:171 |
| XP-342 | library API file-scope pbwa | File-scope Zhang/Jin special function: parabolic cylinder functions W(a,x) and derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:21262; src/FUNCTIONS.f90:171 |
| XP-343 | library API file-scope psi | File-scope Zhang/Jin special function: the PSI function (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:21432; src/FUNCTIONS.f90:171 |
| XP-344 | library API file-scope qstar | File-scope Zhang/Jin special function: Q*mn(-ic) for oblate radial functions with a small argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:21550; src/FUNCTIONS.f90:171 |
| XP-345 | library API file-scope rctj | File-scope Zhang/Jin special function: Riccati-Bessel function of the first kind, and derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:21648; src/FUNCTIONS.f90:171 |
| XP-346 | library API file-scope rcty | File-scope Zhang/Jin special function: Riccati-Bessel function of the second kind, and derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:21763; src/FUNCTIONS.f90:171 |
| XP-347 | library API file-scope refine | File-scope Zhang/Jin special function: an estimate of the characteristic value of Mathieu functions (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:21850; src/FUNCTIONS.f90:171 |
| XP-348 | library API file-scope rmn1 | File-scope Zhang/Jin special function: prolate and oblate spheroidal functions of the first kind (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:21965; src/FUNCTIONS.f90:171 |
| XP-349 | library API file-scope rmn2l | File-scope Zhang/Jin special function: prolate and oblate spheroidal functions, second kind, large CX (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:22187; src/FUNCTIONS.f90:171 |
| XP-350 | library API file-scope rmn2so | File-scope Zhang/Jin special function: oblate radial functions of the second kind with small argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:22374; src/FUNCTIONS.f90:171 |
| XP-351 | library API file-scope rmn2sp | File-scope Zhang/Jin special function: prolate, oblate spheroidal radial functions, kind 2, small argument (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:22514; src/FUNCTIONS.f90:171 |
| XP-352 | library API file-scope rswfo | File-scope Zhang/Jin special function: prolate spheroidal radial function of first and second kinds (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:22753; src/FUNCTIONS.f90:171 |
| XP-353 | library API file-scope rswfp | File-scope Zhang/Jin special function: prolate spheroidal radial function of first and second kinds (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:22848; src/FUNCTIONS.f90:171 |
| XP-354 | library API file-scope scka | File-scope Zhang/Jin special function: expansion coefficients for prolate and oblate spheroidal functions (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:22940; src/FUNCTIONS.f90:171 |
| XP-355 | library API file-scope sckb | File-scope Zhang/Jin special function: expansion coefficients for prolate and oblate spheroidal functions (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:23124; src/FUNCTIONS.f90:171 |
| XP-356 | library API file-scope sdmn | File-scope Zhang/Jin special function: expansion coefficients for prolate and oblate spheroidal functions (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:23244; src/FUNCTIONS.f90:171 |
| XP-357 | library API file-scope segv | File-scope Zhang/Jin special function: the characteristic values of spheroidal wave functions (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:23478; src/FUNCTIONS.f90:171 |
| XP-358 | library API file-scope sphi | File-scope Zhang/Jin special function: spherical Bessel functions in(x) and their derivatives in'(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:23694; src/FUNCTIONS.f90:171 |
| XP-359 | library API file-scope sphj | File-scope Zhang/Jin special function: spherical Bessel functions jn(x) and their derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:23799; src/FUNCTIONS.f90:171 |
| XP-360 | library API file-scope sphk | File-scope Zhang/Jin special function: modified spherical Bessel functions kn(x) and derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:23914; src/FUNCTIONS.f90:171 |
| XP-361 | library API file-scope sphy | File-scope Zhang/Jin special function: spherical Bessel functions yn(x) and their derivatives (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:24004; src/FUNCTIONS.f90:171 |
| XP-362 | library API file-scope stvh0 | File-scope Zhang/Jin special function: the Struve function H0(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:24087; src/FUNCTIONS.f90:171 |
| XP-363 | library API file-scope stvh1 | File-scope Zhang/Jin special function: the Struve function H1(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:24199; src/FUNCTIONS.f90:171 |
| XP-364 | library API file-scope stvhv | File-scope Zhang/Jin special function: the Struve function Hv(x) with arbitrary order v (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:24312; src/FUNCTIONS.f90:171 |
| XP-365 | library API file-scope stvl0 | File-scope Zhang/Jin special function: the modified Struve function L0(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:24497; src/FUNCTIONS.f90:171 |
| XP-366 | library API file-scope stvl1 | File-scope Zhang/Jin special function: the modified Struve function L1(x) (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:24596; src/FUNCTIONS.f90:171 |
| XP-367 | library API file-scope stvlv | File-scope Zhang/Jin special function: the modified Struve function Lv(x) with arbitary order (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:24691; src/FUNCTIONS.f90:171 |
| XP-368 | library API file-scope vvla | File-scope Zhang/Jin special function: parabolic cylinder function Vv(x) for large arguments (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:24849; src/FUNCTIONS.f90:171 |
| XP-369 | library API file-scope vvsa | File-scope Zhang/Jin special function: parabolic cylinder function V(nu,x) for small arguments (compiled with FUNCTIONS.f90; not in MODULE FUNCTIONS public list). | E3 src/functions_special_funcs.f90:24929; src/FUNCTIONS.f90:171 |
| XP-370 | CLI linspace | Prints L evenly spaced reals from wmin/a to wmax/b (optional range=a:b:L). | E2 numutils/src/linspace.f90:12-26 |
| XP-371 | CLI logspace | Prints L logarithmically spaced reals from wmin to wmax by calling TOOLS::logspace. | E3 numutils/src/logspace.f90:31-48 |
| XP-372 | CLI arange | Prints L real numbers equal to 1..L via TOOLS::arange. | E2 numutils/src/arange.f90:9-21 |
| XP-373 | CLI fermi | Reads reals from stdin and prints x, fermi(x,beta). | E2 numutils/src/fermi.f90:17-30 |
| XP-374 | CLI deriv | Reads Y or X,Y from stdin and prints a finite-difference derivative (TOOLS::deriv). | E2 numutils/src/deriv.f90:18-32 |
| XP-375 | CLI wmatsubara | Prints L Matsubara frequencies pi/beta*(2*arange-1), optionally repeated. | E2 numutils/src/wmatsubara.f90:10-24 |
| XP-376 | CLI kdensity | Reads samples from stdin and prints a Gaussian KDE (Silverman's rule). | E2 numutils/src/kdensity.f90:25-41 |
| XP-377 | CLI histogram | Reads samples from stdin and prints a binned histogram via STATISTICS. | E2 numutils/src/histogram.f90:21-35 |
| XP-378 | CLI numstat | Reads 1..ncol columns from stdin and prints selected sample moments/locations. | E2 numutils/src/numstat.f90:34-57 |
| XP-379 | CLI splot | Reads Z(:) from stdin and writes a 3D gnuplot data file and script. | E2 numutils/src/splot.f90:23-43 |
| XP-380 | CLI spline | Reads Y or X,Y from stdin and prints cubic/linear/polynomial interpolation on a finer mesh. | E2 numutils/src/spline.f90:18-34 |
| XP-381 | CLI pade | Reads a Matsubara Green's function file and writes Pade continuation on the real axis. | E2 numutils/src/pade.f90:23-39 |
| XP-382 | CLI fftgf | Reads a complex array and writes a selected Green's-function FFT (fw/bw/rt2rw/rw2rt/iw2tau/tau2iw). | E2 numutils/src/fftgf.f90:23-49 |
| XP-383 | CLI random | Prints N random integer/real/complex samples on [min,max]. | E2 numutils/src/random.f90:14-30 |
| XP-384 | CLI func | EXPERIMENTAL: evaluates a libmatheval expression f(x) on reals from stdin. | E2 numutils/src/func.f90:19-34 |
| XP-385 | CLI ffcmplx | Reads X,imG,reG (or swapped) and writes X, abs(G), phase(G). | E2 numutils/src/ffcmplx.f90:15-32 |
| XP-386 | job scripts/build.sh | Configures env, selects FFT backend (default NR), builds libscifor.a, and links .bin/scifor-fidelity. | E3 scripts/build.sh:79-97 |
| XP-387 | job scripts/fidelity.sh | Runs scifor-fidelity and numerically compares five sections to golden files (tol default 1e-10). | E3 scripts/fidelity.sh:161-184 |
| XP-388 | CLI program scifor_fidelity | Fidelity driver: prints linspace-5, logspace-5, arange-5, fermi-beta100, and deriv-xy2 sections. | E3 fidelity/driver.f90:1-66 |
| XP-389 | job bin/sciforvars.sh | Exports SFLIB/SFINCLUDE and sources etc/library.conf so the library can be built and linked. | E3 bin/sciforvars.sh:3-30 |

## 2. Open inventory questions

- [ ] Is `diagPAM` (`numutils/src/Makefile:20-22`) a retired CLI whose sources were never vendored, or a missing file that should be recovered?
- [ ] When `scripts/build.sh` selects `FFT_BACKEND=MKL` or `FFTW3`, should `FFTGF::cfft_1d_ex` (and the missing NR-only `four1`/`cosft2`/`realft` on those backends) be treated as distinct paths or as the same FFTGF surface with a different implementation?
- [ ] Are the 165 file-scope Zhang/Jin procedures in-scope as independently portable APIs, or only as internals of the `FUNCTIONS.f90` compilation unit? They are linker-visible in `libscifor.a` and documented in the `FUNCTIONS.f90` header, but they are not `MODULE FUNCTIONS` public symbols and are not reached via `use SCIFOR`.
- [ ] Should `integrate_d_quadpack.f90` remain suspected-dead? It is compiled into `libscifor.a`, but `INTEGRATE.f90` does not call `qag*`.
- [ ] Was `SPLINE::interp_gtau` an unfinished export, a renamed routine, or a missing include?

## Appendix. Inactive, unknown, and retired paths

| ID | Trigger | Status | Why classified this way | Evidence |
|----|---------|--------|-------------------------|----------|
| XP-390 | CLI vfplot | suspected-dead | Source and Makefile target exist but the program USEs DLPLOT, which is not in this tree; DISLIN/DLPLOT is commented out in include/sfmake.inc; vfplot is not in numutils `all`. | E3 numutils/src/vfplot.f90:1-5; numutils/src/Makefile:8,84-86; include/sfmake.inc:57-65 |
| XP-391 | CLI diagPAM | unknown | numutils Makefile has a diagPAM target that compiles dfftpack.o + diagPAM.f90, but neither source is in this checkout. | E3 numutils/src/Makefile:20-22 |
| XP-392 | job bin/setup_sf.sh | suspected-dead | Interactive installer expects $SFROOT/sf, local/{blas,lapack,fftw3}, and bin/mylibvars.sh, none of which exist in this checkout. | E3 bin/setup_sf.sh:7-16,204-213 |
| XP-393 | library API file-scope src/CHRPACK.f90 | suspected-dead | Large character-pack procedure collection is present under src/ but is not a src/Makefile allmod target, so it is not compiled into libscifor.a. | E3 src/Makefile:5; src/CHRPACK.f90:1 |
| XP-394 | library API M_UNIINV::uniinv | suspected-dead | Standalone module exists but is not compiled by src/Makefile; TOOLS inlines the same ranking code. tools_sort1d.f90 has commented USE M_UNISTA. | E3 src/uniinv.f90:10-22; src/Makefile:5; src/tools_sort1d.f90:9 |
| XP-395 | library API M_UNISTA::unista | suspected-dead | Standalone module exists but is not compiled by src/Makefile; TOOLS inlines the same unique-stable code. | E3 src/unista.f90:4-13; src/Makefile:5 |
| XP-396 | library API file-scope src/random_routines.f90 | suspected-dead | Alan Miller generators (random_normal, random_gamma, ...) exist, but RANDOM.f90 comments out the include so they are not compiled. | E3 src/RANDOM.f90:291; src/random_routines.f90:7 |
| XP-397 | library API FFTGF::cfft_1d_ex | unknown | Public on the MKL and FFTW3 FFTGF backends, absent from the checkout default (FFTGF.f90 -> FFTGF_NR.f90). scripts/build.sh can select those backends. | E3 src/FFTGF_MKL.f90:11,66-75; src/FFTGF_FFTW3.f90:5; src/FFTGF_NR.f90:5-11; scripts/build.sh:50-62 |
| XP-398 | library API file-scope integrate_d_quadpack.f90 (qag, qags, qagi, qagp, qawc, qawf, qawo, qaws, and helpers) | suspected-dead | Compiled into libscifor.a by the INTEGRATE Makefile rule, but INTEGRATE.f90 public API (trapz/simps/kronig/finter) does not call qag*. | E3 src/Makefile:149-153; src/INTEGRATE.f90:62-70; src/integrate_d_quadpack.f90:1-40 |
| XP-399 | library API SPLINE::interp_gtau | unknown | SPLINE declares public :: interp_gtau but no matching subroutine or function body exists in SPLINE.f90 or its includes. | E3 src/SPLINE.f90:38 |
| XP-400 | library API module FFTGF_FFTPACK | suspected-dead | Alternate FFTGF implementation is not one of the three backends scripts/build.sh will symlink (NR|FFTW3|MKL). | E3 src/FFTGF_FFTPACK.f90:1-6; scripts/build.sh:50-62 |

## Errata

None.

## Links

- Dependency graph: [`docs/modernization/dependency-graph.md`](dependency-graph.md)
- Assessment: [`docs/modernization/ASSESSMENT.md`](ASSESSMENT.md)
