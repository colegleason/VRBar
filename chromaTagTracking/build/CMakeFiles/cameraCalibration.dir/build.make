# CMAKE generated file: DO NOT EDIT!
# Generated by "Unix Makefiles" Generator, CMake Version 3.0

#=============================================================================
# Special targets provided by cmake.

# Disable implicit rules so canonical targets will work.
.SUFFIXES:

# Remove some rules from gmake that .SUFFIXES does not remove.
SUFFIXES =

.SUFFIXES: .hpux_make_needs_suffix_list

# Suppress display of executed commands.
$(VERBOSE).SILENT:

# A target that is always out of date.
cmake_force:
.PHONY : cmake_force

#=============================================================================
# Set environment variables for the build.

# The shell in which to execute make rules.
SHELL = /bin/sh

# The CMake executable.
CMAKE_COMMAND = /usr/local/Cellar/cmake/3.0.1/bin/cmake

# The command to remove a file.
RM = /usr/local/Cellar/cmake/3.0.1/bin/cmake -E remove -f

# Escaping for special characters.
EQUALS = =

# The top-level source directory on which CMake was run.
CMAKE_SOURCE_DIR = /Users/cole/VRBar/chromaTagTracking

# The top-level build directory on which CMake was run.
CMAKE_BINARY_DIR = /Users/cole/VRBar/chromaTagTracking/build

# Include any dependencies generated for this target.
include CMakeFiles/cameraCalibration.dir/depend.make

# Include the progress variables for this target.
include CMakeFiles/cameraCalibration.dir/progress.make

# Include the compile flags for this target's objects.
include CMakeFiles/cameraCalibration.dir/flags.make

CMakeFiles/cameraCalibration.dir/cameraCalibration/camera_calibration.cpp.o: CMakeFiles/cameraCalibration.dir/flags.make
CMakeFiles/cameraCalibration.dir/cameraCalibration/camera_calibration.cpp.o: ../cameraCalibration/camera_calibration.cpp
	$(CMAKE_COMMAND) -E cmake_progress_report /Users/cole/VRBar/chromaTagTracking/build/CMakeFiles $(CMAKE_PROGRESS_1)
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Building CXX object CMakeFiles/cameraCalibration.dir/cameraCalibration/camera_calibration.cpp.o"
	/usr/bin/c++   $(CXX_DEFINES) $(CXX_FLAGS) -o CMakeFiles/cameraCalibration.dir/cameraCalibration/camera_calibration.cpp.o -c /Users/cole/VRBar/chromaTagTracking/cameraCalibration/camera_calibration.cpp

CMakeFiles/cameraCalibration.dir/cameraCalibration/camera_calibration.cpp.i: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Preprocessing CXX source to CMakeFiles/cameraCalibration.dir/cameraCalibration/camera_calibration.cpp.i"
	/usr/bin/c++  $(CXX_DEFINES) $(CXX_FLAGS) -E /Users/cole/VRBar/chromaTagTracking/cameraCalibration/camera_calibration.cpp > CMakeFiles/cameraCalibration.dir/cameraCalibration/camera_calibration.cpp.i

CMakeFiles/cameraCalibration.dir/cameraCalibration/camera_calibration.cpp.s: cmake_force
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --green "Compiling CXX source to assembly CMakeFiles/cameraCalibration.dir/cameraCalibration/camera_calibration.cpp.s"
	/usr/bin/c++  $(CXX_DEFINES) $(CXX_FLAGS) -S /Users/cole/VRBar/chromaTagTracking/cameraCalibration/camera_calibration.cpp -o CMakeFiles/cameraCalibration.dir/cameraCalibration/camera_calibration.cpp.s

CMakeFiles/cameraCalibration.dir/cameraCalibration/camera_calibration.cpp.o.requires:
.PHONY : CMakeFiles/cameraCalibration.dir/cameraCalibration/camera_calibration.cpp.o.requires

CMakeFiles/cameraCalibration.dir/cameraCalibration/camera_calibration.cpp.o.provides: CMakeFiles/cameraCalibration.dir/cameraCalibration/camera_calibration.cpp.o.requires
	$(MAKE) -f CMakeFiles/cameraCalibration.dir/build.make CMakeFiles/cameraCalibration.dir/cameraCalibration/camera_calibration.cpp.o.provides.build
.PHONY : CMakeFiles/cameraCalibration.dir/cameraCalibration/camera_calibration.cpp.o.provides

CMakeFiles/cameraCalibration.dir/cameraCalibration/camera_calibration.cpp.o.provides.build: CMakeFiles/cameraCalibration.dir/cameraCalibration/camera_calibration.cpp.o

# Object files for target cameraCalibration
cameraCalibration_OBJECTS = \
"CMakeFiles/cameraCalibration.dir/cameraCalibration/camera_calibration.cpp.o"

# External object files for target cameraCalibration
cameraCalibration_EXTERNAL_OBJECTS =

cameraCalibration: CMakeFiles/cameraCalibration.dir/cameraCalibration/camera_calibration.cpp.o
cameraCalibration: CMakeFiles/cameraCalibration.dir/build.make
cameraCalibration: /usr/local/lib/libopencv_videostab.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_video.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_ts.a
cameraCalibration: /usr/local/lib/libopencv_superres.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_stitching.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_photo.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_ocl.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_objdetect.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_nonfree.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_ml.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_legacy.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_imgproc.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_highgui.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_gpu.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_flann.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_features2d.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_core.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_contrib.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_calib3d.2.4.9.dylib
cameraCalibration: ../libapriltag.a
cameraCalibration: /usr/local/lib/libopencv_nonfree.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_ocl.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_gpu.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_photo.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_objdetect.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_legacy.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_video.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_ml.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_calib3d.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_features2d.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_highgui.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_imgproc.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_flann.2.4.9.dylib
cameraCalibration: /usr/local/lib/libopencv_core.2.4.9.dylib
cameraCalibration: CMakeFiles/cameraCalibration.dir/link.txt
	@$(CMAKE_COMMAND) -E cmake_echo_color --switch=$(COLOR) --red --bold "Linking CXX executable cameraCalibration"
	$(CMAKE_COMMAND) -E cmake_link_script CMakeFiles/cameraCalibration.dir/link.txt --verbose=$(VERBOSE)

# Rule to build all files generated by this target.
CMakeFiles/cameraCalibration.dir/build: cameraCalibration
.PHONY : CMakeFiles/cameraCalibration.dir/build

CMakeFiles/cameraCalibration.dir/requires: CMakeFiles/cameraCalibration.dir/cameraCalibration/camera_calibration.cpp.o.requires
.PHONY : CMakeFiles/cameraCalibration.dir/requires

CMakeFiles/cameraCalibration.dir/clean:
	$(CMAKE_COMMAND) -P CMakeFiles/cameraCalibration.dir/cmake_clean.cmake
.PHONY : CMakeFiles/cameraCalibration.dir/clean

CMakeFiles/cameraCalibration.dir/depend:
	cd /Users/cole/VRBar/chromaTagTracking/build && $(CMAKE_COMMAND) -E cmake_depends "Unix Makefiles" /Users/cole/VRBar/chromaTagTracking /Users/cole/VRBar/chromaTagTracking /Users/cole/VRBar/chromaTagTracking/build /Users/cole/VRBar/chromaTagTracking/build /Users/cole/VRBar/chromaTagTracking/build/CMakeFiles/cameraCalibration.dir/DependInfo.cmake --color=$(COLOR)
.PHONY : CMakeFiles/cameraCalibration.dir/depend

