/*{
	"CREDIT": "xdxst",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"generator"
	],
	"INPUTS": [
		{
			"NAME": "colorInput",
			"TYPE": "color",
			"DEFAULT": [
				0.0,
				0.0,
				1.0,
				1.0
			]
		},
		{
			"NAME": "peaks",
			"TYPE": "float",
			"DEFAULT": 10,
			"MIN": 0,
			"MAX": 100
		},
		{
			"NAME": "horizonDistance",
			"TYPE": "float",
			"DEFAULT": 100,
			"MIN": 1,
			"MAX": 1000
		},
		{
			"NAME": "fieldOfView",
			"TYPE": "float",
			"DEFAULT": 12.5,
			"MIN": 1,
			"MAX": 25
		},
		{
			"NAME": "zoomSpeed",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": 0,
			"MAX": 0.5
		},
		{
			"NAME": "rotationSpeed",
			"TYPE": "float",
			"DEFAULT": 1,
			"MIN": 0,
			"MAX": 5
		},
		{
			"NAME": "smoothStep1",
			"TYPE": "point2D",
			"DEFAULT": [0.2, 0.36],
			"MIN": [0,0],
			"MAX": [1,1]
		},		
		{
			"NAME": "smoothStep2",
			"TYPE": "point2D",
			"DEFAULT": [0.89,1],
			"MIN": [0,0],
			"MAX": [1,1]
		}
		
	]
}*/

#define TAU 2.0*3.1415926535897932384626433832795

float signal(float time, float shift) {
  return pow(mod(time*zoomSpeed+shift,1.5), fieldOfView);
}

float circle(vec2 ctr, float scale, float time, float shift) {
  return (pow(ctr.x*signal(time, shift), 2.) + pow(ctr.y*signal(time, shift), 2.))*scale+
  .022*cos(atan(ctr.x,ctr.y)*floor(peaks)+TIME*rotationSpeed*TAU);
}

float ring(vec2 ctr, float scale, float time, float shift) {
  return smoothstep(smoothStep1.x, smoothStep1.y, circle(ctr, scale, time, shift)*12.0) * 
  smoothstep(smoothStep2.x, smoothStep2.y, 1. - circle(ctr, scale, time, shift));
}

void main() {
    vec2 st = gl_FragCoord.xy/(RENDERSIZE.xy);
    vec2 ctr = (st - 0.5)*2.;
    vec3 rgb = vec3(
                    ring(ctr, horizonDistance, -TIME, .0) + ring(ctr, horizonDistance, -TIME, .75) + 
                    ring(ctr, horizonDistance, -TIME, .375) + ring(ctr, horizonDistance, -TIME, 1.125),
                    ring(ctr, horizonDistance, -TIME, .5) + ring(ctr, horizonDistance, -TIME, 1.25) + 
                    ring(ctr, horizonDistance, -TIME, .125) + ring(ctr, horizonDistance, -TIME, .875),
                    ring(ctr, horizonDistance, -TIME, 1.0) + ring(ctr, horizonDistance, -TIME, .25) + 
                    ring(ctr, horizonDistance, -TIME, .625) + ring(ctr, horizonDistance, -TIME, 1.375)
                    );
    gl_FragColor = vec4(
    rgb
    ,
    1.0);
}