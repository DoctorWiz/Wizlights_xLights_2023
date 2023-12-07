/*{
    "CREDIT": "by mojovideotech",
    "CATEGORIES" : [
    	"generator",
    	"circles",
    	"dots"
  	],
  	"DESCRIPTION": "",
  	"ISFVSN" : "2",
  	"INPUTS" : [
    {
        "NAME" :        "seed",
        "TYPE" :        "float",
        "DEFAULT" :     601,
        "MIN" :         5,
        "MAX" :         2867
    },
    {
        "NAME" :        "scale",
        "TYPE":         "float",
        "DEFAULT" :     -36.0,
        "MIN" :         -50.0,
        "MAX" :         12.0
    },
    {
        "NAME" :        "radius",
        "TYPE" :        "float",
        "DEFAULT" :     0.33,
        "MIN" :         0.01,
        "MAX" :         0.45
    },
    {
        "NAME" :        "pulse",
        "TYPE" :        "float",
        "DEFAULT" :     0.333,
        "MIN" :         -5.0,
        "MAX" :         5.0
    },
    {
        "NAME" :        "soft",
        "TYPE" :        "float",
        "DEFAULT" :     0.067,
        "MIN" :         0.01,
        "MAX" :         0.5
    },
    {
     	"NAME"	: 		"C1",
       	"TYPE"	: 		"float",    
       	"DEFAULT": 		1.0,
       	"MAX" : 		1,
	   	"MIN"	: 		0
  	},
  	{
  	 	"NAME"	: 		"C2",
      	"TYPE"	: 		"float",
    	"DEFAULT"	: 	0.75,
       	"MAX"	: 		1,
    	"MIN"	: 		0
  	},
 	{
     	"NAME"	: 		"C3",
    	"TYPE"	: 		"float",
     	"DEFAULT"	: 	0.275,
     	"MAX"	: 		1,
       	"MIN"	: 		0
   	},
    {
    	"NAME" :        "brightness",
        "TYPE" :        "float",
        "DEFAULT" :     -0.33,
        "MIN" :        -0.33,
        "MAX" :         1.0
    },
    {
        "NAME" :        "gamma",
        "TYPE" :        "float",
        "DEFAULT" :     0.5,
        "MIN" :         0.0,
        "MAX" :         1.0
    },
    {
        "NAME" :        "rate",
        "TYPE" :        "float",
        "DEFAULT" :     0.23,
        "MIN" :         0.0,
        "MAX" :         5.0
    },
    {
        "NAME" :        "scroll",
        "TYPE" :        "float",
        "DEFAULT" :     0.025,
        "MIN" :         -0.05,
        "MAX" :         0.05
    }
  ]
}
*/

////////////////////////////////////////////////////////////
// DotzMatrix  by mojovideotech
//
// based on:
// shadertoy.com//3tS3zV
//
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0
////////////////////////////////////////////////////////////

float N11(float p) {
	vec2 p2 = fract(p * vec2(5.3983, 5.4427));
    	p2 += dot(p2.yx, p2.xy + vec2(21.5351, 14.3137));
	return fract(p2.x * p2.y);
}

float N12(vec2 p) { 
    return fract(sin(dot(p, vec2(12.9898, 78.233)+seed)+(rate*TIME*N11(pulse)))*43758.5453);   
}

float makeCircle(vec2 p, float r) {
    float c = length(p);
    return 1.0-smoothstep(r-soft, r+soft, c);
}

void main() 
{
    vec2 uv = 0.5*(gl_FragCoord.xy/RENDERSIZE.xy+0.5);
    uv.x*=RENDERSIZE.x/RENDERSIZE.y;
	uv.y+=TIME*scroll;
    vec3 col = vec3(1.0), blk = vec3(0.0), col2 = vec3(C1, C2, C3);
    vec3 lg = vec3(0.66)-brightness;
    vec3 dg = vec3(0.4)-brightness;
    float size = 25.0 - scale;
    float sd = N12(floor(uv*-size));
    uv = fract(uv*size)-0.5;
    float circ = makeCircle(uv, radius);
    col = mix(col, col2, circ*step(sd, 0.33) );
    col = mix(col, lg, circ*step(0.33, sd));
    col = mix(col, dg, circ*step(1.0-sd, 0.33));

    gl_FragColor = vec4(sqrt(mix(1.0-col,blk,0.5-gamma)),1.0);
}
