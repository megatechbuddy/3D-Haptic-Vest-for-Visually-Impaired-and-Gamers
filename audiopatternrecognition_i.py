#!/usr/bin/env python
# Python 2.7 code to analyze sound and interface with Parallax Propeller
import pyaudio
import numpy
import audioop
import sys
import math
import struct
import os
import serial
ser = serial.Serial('/dev/ttyUSB0', 115200)
'''
Sources
http://www.swharden.com/blog/2010-03-05-realtime-fft-graph-of-audio-wav-file-or-microphone-input-with-python-scipy-and-wckgraph/
http://macdevcenter.com/pub/a/python/2001/01/31/numerically.html?page=2
'''

def arduino_soundlight():
	chunk=2**8# Change if too fast/slow, never less than 2**11
	samplerate=14400#Most audio sources that you will use will be set to 44100

	device = 2# Device Number

	# Start audio stream
	p = pyaudio.PyAudio()
	stream = p.open(format = pyaudio.paInt16,channels = 1,rate = samplerate,input = True,frames_per_buffer = chunk,input_device_index = device)

	print "Starting, use Ctrl+C to stop"
	d = 0
	while True:
		# Get Audio Data
		data= stream.read(chunk)

		# Do FFT
		levels = calculate_levels(data, chunk, samplerate)

		b=0
		d=0
		for level in levels:
			b= b + 1
			if level>0 and b== 1:
				d=1
				
		if d==1:
			#print "Fired"
			ser.write('9999\n\r')
		elif d==0:
			ser.write('8888\n\r')
			#print "cleared"

def calculate_levels(data, chunk, samplerate):
	# Use FFT to calculate volume for each frequency
	global MAX

	# Convert raw sound data to Numpy array
	fmt = "%dH"%(len(data)/2)
	data2 = struct.unpack(fmt, data)
	data2 = numpy.array(data2, dtype='h')
	
	# Apply FFT
	fourier = numpy.fft.fft(data2)
	ffty = numpy.abs(fourier[0:len(fourier)/2])/1000
	ffty1=ffty[:len(ffty)/2]
	ffty2=ffty[len(ffty)/2::]+2
	ffty2=ffty2[::-1]
	ffty=ffty1+ffty2
	ffty=numpy.log(ffty)-2
	
	fourier = list(ffty)[4:-4]
	fourier = fourier[:len(fourier)/2]

	size = len(fourier)

	# Add up for 6 lights
	levels = [sum(fourier[i:(i+size/6)]) for i in xrange(0, size, size/6)][:6]

	return levels
	
if __name__ == '__main__':
	#list_devices()
	arduino_soundlight()#I Don't know why I have to use this yet

