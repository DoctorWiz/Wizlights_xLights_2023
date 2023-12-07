/*
{
  "CATEGORIES" : ["Generator"

  ],
  "DESCRIPTION" : "Vortex in colors",
  "INPUTS" : [
    
    {
      "NAME" : "timeScale",
      "TYPE" : "float",
      "MAX" : 10,
      "DEFAULT" : 0.50,
      "MIN" : 0.1
    },
    {
      "NAME" : "r",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.5,
      "MIN" : 0
    },
    {
      "NAME" : "g",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.5,
      "MIN" : 0
    },
    {
      "NAME" : "b",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.1,
      "MIN" : 0
    },
    {
      "NAME" : "scale",
      "TYPE" : "float",
      "MAX" : 100,
      "DEFAULT" : 0.1,
      "MIN" : 0
    },
    
    {
      "NAME" : "LIGHT",
      "TYPE" : "float",
      "MAX" : -0.001,
      "DEFAULT" : -0.05,
      "MIN" : -0.15
    },
    {
      "NAME" : "radius",
      "TYPE" : "float",
      "MAX" : 10,
      "DEFAULT" : 1.472574,
      "MIN" : 0.1
    },
    {
      "NAME" : "tilt",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" :-0.45,
      "MIN" : -1
    },
    {
      "NAME" : "roll",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : -0.54,
      "MIN" : -1
    },
    {
      "NAME" : "yaw",
      "TYPE" : "float",
      "MAX" : 1,
      "DEFAULT" : 0.2,
      "MIN" : -1
    }
  ],
  "ISFVSN" : "2",
  "CREDIT" : "by thedantheman"
}
*/

#ifdef GL_ES
precision mediump float;
#endif


// srtuss, 2014
// Ported from www.shadertoy.com

#define PI 3.14159265

#define ITER 5

vec2 rotate(vec2 p, float a)
	{
	return vec2(p.x * cos(a) - p.y * sin(a), p.x * sin(a) + p.y * cos(a));
	}

vec2 circuit(vec2 p)
	{
	p = fract(p);
	float r =10.123;
	float v = 0.0, g = 0.0;
	float test = 0.0;
	r = fract(r * 84.928);
	float cp, d;
	
	d = p.x;
	g += pow(clamp(1.0 - abs(d), 0.0, 1.0), 1000.0);
	d = p.y;
	g += pow(clamp(1.0 - abs(d), 0.0, 1.0), 1000.0);
	d = p.x - 1.0;
	g += pow(clamp(1.0 - abs(d), 0.0, 1.0), 1000.0);
	d = p.y - 1.0;
	g += pow(clamp(1.0 - abs(d), 0.0, 1.0), 10000.0);
	
	for(int i = 0; i < ITER; i ++)
		{
		cp = 0.5 + (r - 0.5) * 0.9;
		d = p.x - cp;
		g += pow(clamp(1.0 - abs(d), 0.0, 1.0), 160.0);
		if(d > 0.0)
			{
			r = fract(r * 4829.013);
			p.x = (p.x - cp) / (1.0 - cp);
			v += 1.0;
			test = r;
			}
		else
			{
			r = fract(r * 1239.528);
			p.x = p.x / cp;
			test = r;
			}
		p = p.yx;
		}
	v /= float(ITER);
	return vec2(v, g);
	}

float box(vec2 p, vec2 b, float r)
	{
	return length(max(abs(p) - b, 0.0)) -r ;
	}

float rand(float p)
	{
	return fract(sin(p * 591.32) * 43758.5357);
	}

float rand2(vec2 p)
	{
	return fract(sin(dot(p.xy, vec2(12.9898, 78.233))) * 43758.5357);
	}

vec2 rand2(float p)
	{
	return fract(vec2(sin(p * 591.32), cos(p * 391.32)));
	}

vec3 sky(vec3 rd, float t)
	{
	float u = atan(rd.z, rd.x) / PI / abs(sin(radius))*radius;
	float v = rd.y / length(rd.xz);
	float fg = exp(LIGHT * abs(v));
	vec2 ca = circuit(vec2(u, (v - t * 2.1) * 1.8));
	//valeurs lignes précédentes importance de la bande bleu clair
	vec2 cb = circuit(vec2(-u, (v - t * 8.1) * 0.1));
	//valeurs ligne précédente importance de la bande verte
	float c = (ca.x - ca.y * 0.5) + cb.y * 0.7;
	//effet de bruit lumineux
	vec3 glow = pow(vec3(c), vec3(0.2, 3.5, .3) * 2.0);
	//en montant les valeurs de la ligne précédente, on augmente l'effet de brillance - contour fluo
	vec2 cr = vec2(u, (v - t * 5.0) * 0.03);
	float crFr = fract(cr.y);
	float r = smoothstep(0.8, 0.1, abs(crFr * 2.0 - 1.0));
	float vo = 0.0, gl = 0.0;
	for(int i = 0; i < 6; i ++)
		{
		float id = float(i);
		vec2 off = rand2(id);
		vec2 pp = vec2(fract(cr.x * 5.0 + off.x + t * 8.0 * (0.5 + rand(id))) - 0.5, fract(cr.y * 12.0 + off.y * 0.2) - 0.5);
		float di = box(pp, vec2(0.2, 0.01), 0.02);
		vo += smoothstep(0.999, 1.0, 1.0 - di);
		gl += exp(max(di, 0.0) * -16.0);
		}
	vo = pow(vo * 0.4, 2.0);
	vec3 qds = vec3(1.0);
	vec3 col = mix(glow, qds, clamp(vo, 0.0, 1.0)) + vec3(0.05, 0.05, 0.05) * gl * 0.5;
	return col + (1.0 - fg);
	}

vec3 colorset(float v)
	{
	return pow(vec3(v),vec3(r, b , g ));
	}



vec3 pixel(vec2 uv)
	{
	uv /= RENDERSIZE.xy;
	uv = uv * 2.0 - 1.0;
	uv.x *= RENDERSIZE.x / RENDERSIZE.y;
	vec3 ro = vec3(1.0, 1.0, 0.0);
	vec3 rd = normalize(vec3(uv, scale));
	float t = TIME*timeScale;
	float down = PI / 2.;
	//en remplaçant sin par log on change la forme du tunnel, la section à la place d etre circulaire est parabolique
	rd.yz = rotate(rd.yz, sin(tilt*PI));
	rd.xz = rotate(rd.xz,  sin(roll)*PI);
	rd.xy = rotate(rd.xy, sin(yaw)*PI);
	vec3 col = sky(rd, t);
	return pow(col, vec3(1.9)) * 1.3;
	}



void main()
	{
	vec2 uv = gl_FragCoord.xy;
	vec3 col;
	vec2 h = vec2(0.0, 0.0);
	col = pixel(uv);
	col += pixel(uv + h.xy);
	col += pixel(uv + h.yx);
	col += pixel(uv + h.xx);
	col /= 4.0;

	gl_FragColor = vec4(col, 1.0);
	gl_FragColor.b *= b ;
	gl_FragColor.g *= g ;
	gl_FragColor.r *= r ;
		
	}