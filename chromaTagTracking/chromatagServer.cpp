
// Standard C Libraries
#include <stdio.h>
#include <stdint.h>
#include <inttypes.h>
#include <ctype.h>
#include <unistd.h>

// Server Libraries
#include <errno.h>
#include <string.h>
#include <unistd.h>
#include <netdb.h>
#include <sys/socket.h>
#include <netinet/in.h>

// Apriltags libraries
#include "apriltags/apriltag.h"
#include "apriltags/common/image_u8.h"
#include "apriltags/tag36h11.h"
#include "apriltags/tag36h10.h"
#include "apriltags/tag36artoolkit.h"
#include "apriltags/tag25h9.h"
#include "apriltags/tag25h7.h"

#include "apriltags/common/zarray.h"
#include "apriltags/common/getopt.h"

// Our extensions for chromatags
#include "lib/rgb2lab.hpp" // functions to convert to rgb to lab, and seperate color channels
#include "lib/pnm2mat.hpp" // functions to convert pnm to and from mat

#define MY_PORT		9499
#define MAXBUF		1024

void handle_session(int session_fd){
  
  char buffer[MAXBUF];
  
  bool showGradientEdges = false;
  bool found = false;
  int w = 854;
  int h = 480;
  
  VideoCapture cap(0); // open the default camera
  Size size(w,h);  // size of desired frame origionally 1280x720, 1024x576, 854x480
  if(!cap.isOpened())  // check if camera opened
    return;
  
  Mat a, b, g, frame, src;
  
  /* From apriltag_demo.c */
  
  int maxiters = 5;
  const int hamm_hist_max = 10;
  int quiet = 0;
  
  apriltag_family_t *tf = tag36h11_create();                // Apriltag family 36h11, can change
  tf->black_border = 1;                                     // Set tag family border size
  
  apriltag_detector_t *td = apriltag_detector_create();     // Apriltag detector
  apriltag_detector_add_family(td, tf);                     // Add apriltag family
  
  td->quad_decimate = 1.0;                                  // Decimate input image by factor
  td->quad_sigma = 0.0;                                     // No blur (I think)
  td->nthreads = 4;                                         // 4 treads provided
  td->debug = 0;                                            // No debuging output
  td->refine_decode = 0;                                    // Don't refine decode
  td->refine_pose = 0;                                      // Don't refine pose
  
  // Output variables
  char imgSize[20];
  char renderTime[20];
  char detectString[50];
  char convertTime[50];
  char displayString[120];
  char outputString[120];
  char locationString[120];
  double time_taken = 0.0;
  
  long double totalFPS = 0.0;
  double count = 0.0;
  
  /* End of apriltag_demo.c */
  
  while(1){
    
    
    clock_t t;
    t = clock();
    
    cap >> src;                                               // Get a new frame from camera
    if(found){
      resize(src,frame,size);                                 // Resize to smaller image if tag found
      //resize(frame, src, size);
    }else{
      frame = src;                                            // Keep standard image if no tag
    }
    //frame = RGB2YUV(frame);                                 // Just for comparison
    frame = RGB2LAB(frame);                                   // Returns lab space
    frame = alphaLAB(frame);                                  // Look at only a channel
    
    // resize(frame,src,src.size());
    
    if(showGradientEdges){
      src = gradientEdges(src);                               // Show graprintfnt for fun
    }
    
    // determine time to convert
    time_taken = ((double)(clock() - t))/(CLOCKS_PER_SEC/1000);
    sprintf(convertTime, "Convert Time: %5.3fms", time_taken);
    
    t = clock();
    
    pnm_t *pnm = mat2pnm(&frame);
    image_u8_t *im = pnm_to_image_u8(pnm);                    // Convert pnm to gray image_u8
    if (im == NULL) {                                         // Error - no image created from pnm
      std::cout << "Error, not a proper pnm" << std::endl;
      return;
    }
    
    /*** Start from origional Apriltags from apriltag_demo.c ***/
    
    int hamm_hist[hamm_hist_max];
    memset(hamm_hist, 0, sizeof(hamm_hist));
    zarray_t *detections = apriltag_detector_detect(td, im);
    
    for (int i = 0; i < zarray_size(detections); i++) {
      
      apriltag_detection_t *det;
      zarray_get(detections, i, &det);
      sprintf(locationString, "Tag Center: (%f,%f)", det->c[0], det->c[1]);
      sprintf(detectString, "detection %2d: id (%2dx%2d)-%-4d, hamming %d, goodness %5.3f, margin %5.3f\n",
              i+1, det->family->d*det->family->d, det->family->h, det->id, det->hamming, det->goodness, det->decision_margin);
      
      hamm_hist[det->hamming]++;
      
      // draws a vertical rectangle around tag, not ideal, but easy to implement
      // det->p[corner][positon], counter clockwise
      Point pt1 = Point(det->p[0][0], det->p[0][1]);
      Point pt2 = Point(det->p[2][0], det->p[2][1]);
      
      // If tag found flag, scale to image size
      if(found){
        Size s = src.size();
        pt1 = Point((det->p[0][0]/w) * s.width, (det->p[0][1]/h) * s.height);
        pt2 = Point((det->p[2][0]/w) * s.width, (det->p[2][1]/h) * s.height);
      }
      cv::rectangle(src, pt1, pt2, cvScalar(102,255,0));
      
      apriltag_detection_destroy(det);
    }
    
    if(zarray_size(detections) < 1){
      found = false;
      sprintf(detectString, "No tag detected");
      sprintf(locationString, "No tag detected");
    }else{
      found = true;
    }
    
    zarray_destroy(detections);
    image_u8_destroy(im);
    
    t = clock() - t;
    time_taken = ((double)t)/(CLOCKS_PER_SEC/1000);
    //printf("ms to render: %5.3f\n", time_taken);
    
    if (!quiet) {
      //timeprofile_display(td->tp);
      totalFPS += (1000.0/time_taken);
      count += 1.0;
      if(count > 150.0){
        totalFPS = 0.0;
        count = 0.0;
      }
      sprintf(displayString, "fps: %2.2Lf, nquads: %d",totalFPS/count, td->nquads);
      //std::cout << displayString;
    }
    
    //for (int i = 0; i < hamm_hist_max; i++)
    //printf("%5d", hamm_hist[i]);
    
    sprintf(renderTime, "Render: %5.3fms", time_taken);
    sprintf(imgSize, "%dx%d", frame.cols, frame.rows);
    sprintf(outputString, "%s %s %s", renderTime, convertTime, imgSize);
    printf("%s %s\r", locationString, outputString);
    
    if (quiet) {
      printf("%12.3f", timeprofile_total_utime(td->tp) / 1.0E3);
    }
    
    printf("\n");
    
    /*** End of origional Apriltags from apriltag_demo.c ***/
    
    // displays fps, edges, segments, quads
    putText(src, displayString, cvPoint(30,30),
            FONT_HERSHEY_COMPLEX_SMALL, 0.8, cvScalar(200,200,250), 1, CV_AA);
    
    // displays render time, convert time, and image size
    putText(src, outputString, cvPoint(30,50),
            FONT_HERSHEY_COMPLEX_SMALL, 0.8, cvScalar(200,200,250), 1, CV_AA);
    
    // Displays any detections (if any)
    putText(src, detectString, cvPoint(30,70),
            FONT_HERSHEY_COMPLEX_SMALL, 0.8, cvScalar(200,200,250), 1, CV_AA);
    
    
    // Displays tag location (if any)
    putText(src, locationString, cvPoint(30,90),
            FONT_HERSHEY_COMPLEX_SMALL, 0.8, cvScalar(150,150,250), 1, CV_AA);
    
    imshow("Display Apriltags", src);
    
    // Write to socket
    sprintf(buffer, "%s %s %s", renderTime, convertTime, imgSize);
    write(session_fd, buffer, MAXBUF);
    
    if(waitKey(30) >= 0) break;
  }
  
  /* deallocate apriltag constructs */
  apriltag_detector_destroy(td);
  tag36h11_destroy(tf);
}

int main(int argc, char * argv[]){
  
  
  /* Server From: http://www.microhowto.info/howto/listen_for_and_accept_tcp_connections_in_c */
  
  const char* hostname=0;
  const char* portname="9499";
  struct addrinfo hints;
  memset(&hints,0,sizeof(hints));
  hints.ai_family=AF_UNSPEC;
  hints.ai_socktype=SOCK_STREAM;
  hints.ai_protocol=0;
  hints.ai_flags=AI_PASSIVE|AI_ADDRCONFIG;
  struct addrinfo* res=0;

  int err=getaddrinfo(hostname,portname,&hints,&res);
  if (err!=0) {
    printf("failed to resolve local socket address (err=%d)",err);
  }

  int server_fd=socket(res->ai_family,res->ai_socktype,res->ai_protocol);
  if (server_fd==-1) {
    printf("%s",strerror(errno));
  }

  int reuseaddr=1;
  if (setsockopt(server_fd,SOL_SOCKET,SO_REUSEADDR,&reuseaddr,sizeof(reuseaddr))==-1) {
    printf("%s",strerror(errno));
  }
  
  if (bind(server_fd,res->ai_addr,res->ai_addrlen)==-1) {
    printf("%s",strerror(errno));
  }
  
  if (listen(server_fd,SOMAXCONN)) {
    printf("failed to listen for connections (errno=%d)",errno);
  }
  
  for (;;) {
    int session_fd=accept(server_fd,0,0);
    if (session_fd==-1) {
      if (errno==EINTR) continue;
      printf("failed to accept connection (errno=%d)",errno);
    }
    pid_t pid=fork();
    if (pid==-1) {
      printf("failed to create child process (errno=%d)",errno);
    } else if (pid==0) {
      close(server_fd);
      handle_session(session_fd);
      close(session_fd);
      _exit(0);
    } else {
      close(session_fd);
    }
  }
  
  for (;;) {
    int session_fd=accept(server_fd,0,0);
    if (session_fd==-1) {
      if (errno==EINTR) continue;
      printf("failed to accept connection (errno=%d)",errno);
    }
    pid_t pid=fork();
    if (pid==-1) {
      printf("failed to create child process (errno=%d)",errno);
    } else if (pid==0) {
      close(server_fd);
      handle_session(session_fd);
      close(session_fd);
      _exit(0);
    } else {
      close(session_fd);
    }
  }
  
  /* End server portion */
  

  freeaddrinfo(res);
  
  return 0;
}
