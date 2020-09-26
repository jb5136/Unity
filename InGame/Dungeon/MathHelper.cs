using UnityEngine;
using System.Collections;

public class MathHelper 
{
	static public Quaternion MatrixToQuaternion(Matrix4x4 m) 
	{ 
		Quaternion q = new Quaternion(); 
		
		q.w = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] + m[1, 1] + m[2, 2])) / 2;
		q.x = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] - m[1, 1] - m[2, 2])) / 2;
		q.y = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] + m[1, 1] - m[2, 2])) / 2;
		q.z = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] - m[1, 1] + m[2, 2])) / 2;
		q.x *= Mathf.Sign(q.x * (m[2, 1] - m[1, 2]));
		q.y *= Mathf.Sign(q.y * (m[0, 2] - m[2, 0]));
		q.z *= Mathf.Sign(q.z * (m[1, 0] - m[0, 1]));
		
		return q; 
	}

	static public float Linear(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value);
	}

    static public Vector3 Linear( Vector3 start, Vector3 end, float value )
    {
        return Vector3.Lerp(start, end, value);
    }

    

	static public float Clerp(float start, float end, float value)
	{
		float min = 0.0f;

                
		float max = 360.0f;

               
		float half = Mathf.Abs((max - min) / 2.0f);

               
		float retval = 0.0f;

               
		float diff = 0.0f;

                
		if ((end - start) < -half)
		{

                    
			diff = ((max - start) + end) * value;

                       
			retval = start + diff;

                
		}else if ((end - start) > half)
		{

                        
			diff = -((max - end) + start) * value;

                       
			retval = start + diff;

                
		}
		else retval = start + (end - start) * value;

                
		return retval;

    }

	static public float Spring(float start, float end, float value)
	{

		value = Mathf.Clamp01(value);

		value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));

		return start + (end - start) * value;

	}

	static public float EaseInQuad(float start, float end, float value)
	{   
		end -= start;

		return end * value * value + start;

	}

	static public float EaseOutQuad(float start, float end, float value)
	{
     
		end -= start;

		return -end * value * (value - 2) + start;

	}




        
	static public float EaseInOutQuad(float start, float end, float value)
	{
   
		value /= .5f;

		end -= start;

		if (value < 1) 
			return end / 2 * value * value + start;

		value--;

		return -end / 2 * (value * (value - 2) - 1) + start;

	}

	static public float EaseInCubic(float start, float end, float value)
	{
		end -= start;

		return end * value * value * value + start;
	}

        
	static public float EaseOutCubic(float start, float end, float value)
	{
          
		value--;
    
		end -= start;
   
		return end * (value * value * value + 1) + start;

        
	}

	static public float EaseInOutCubic(float start, float end, float value)
	{ 
		value /= .5f;

		end -= start;

		if (value < 1) 
			return end / 2 * value * value * value + start;

		value -= 2;
  
		return end / 2 * (value * value * value + 2) + start;

	}

        
	static public float EaseInQuart(float start, float end, float value)
	{
   
		end -= start;
   
		return end * value * value * value * value + start;

	}

	static public float EaseOutQuart(float start, float end, float value)
	{

		value--;

		end -= start;
		return -end * (value * value * value * value - 1) + start;

	}

	static public float EaseInOutQuart(float start, float end, float value)
	{
    
		value /= .5f;

		end -= start;

		if (value < 1) 
			return end / 2 * value * value * value * value + start;

		value -= 2;      
		return -end / 2 * (value * value * value * value - 2) + start;
        
	}

	static public float EaseInQuint(float start, float end, float value)
	{
        
		end -= start;
     
		return end * value * value * value * value * value + start;
	}

	static public float EaseOutQuint(float start, float end, float value)
	{
     
		value--;
       
		end -= start;
   
		return end * (value * value * value * value * value + 1) + start;

	}
        
	static public float EaseInOutQuint(float start, float end, float value)
	{     
		value /= .5f;
     
		end -= start;
   
		if (value < 1) 
			return end / 2 * value * value * value * value * value + start;
     
		value -= 2;
  
		return end / 2 * (value * value * value * value * value + 2) + start;

	}
	
	static public float EaseInSine(float start, float end, float value)
	{       
		end -= start;
     
		return -end * Mathf.Cos(value / 1 * (Mathf.PI / 2)) + end + start;
	}
        
	static public float EaseOutSine(float start, float end, float value)
	{    
		end -= start;      
		return end * Mathf.Sin(value / 1 * (Mathf.PI / 2)) + start;
	}

	static public float EaseInOutSine(float start, float end, float value)
	{
		end -= start;    
		return -end / 2 * (Mathf.Cos(Mathf.PI * value / 1) - 1) + start;
	}


	static public float EaseInExpo(float start, float end, float value)
	{
		end -= start;     
		return end * Mathf.Pow(2, 10 * (value / 1 - 1)) + start;

	}

	static public float EaseOutExpo(float start, float end, float value)
	{
		end -= start;    
		return end * (-Mathf.Pow(2, -10 * value / 1) + 1) + start;
	}

	static public float EaseInOutExpo(float start, float end, float value)
	{     
		value /= .5f;   
		end -= start;     
		if (value < 1) return end / 2 * Mathf.Pow(2, 10 * (value - 1)) + start;     
		value--;   
		return end / 2 * (-Mathf.Pow(2, -10 * value) + 2) + start;

	}

	static public float EaseInCirc(float start, float end, float value)
	{         
		end -= start;   
		return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
	}

	static public float EaseOutCirc(float start, float end, float value)
	{
		value--;    
		end -= start;     
		return end * Mathf.Sqrt(1 - value * value) + start;
	}

	static public float EaseInOutCirc(float start, float end, float value)
	{
		value /= .5f;        
		end -= start;    
		if (value < 1) return -end / 2 * (Mathf.Sqrt(1 - value * value) - 1) + start;       
		value -= 2;
		return end / 2 * (Mathf.Sqrt(1 - value * value) + 1) + start;
	}
	
	static public float Bounce(float start, float end, float value)
	{
       
		value /= 1f;
      
		end -= start;
     
		if (value < (1 / 2.75f))
		{          
			return end * (7.5625f * value * value) + start;
		}
		else if (value < (2 / 2.75f))
		{      
			value -= (1.5f / 2.75f);      
			return end * (7.5625f * (value) * value + .75f) + start;
		}
		else if (value < (2.5 / 2.75))
		{     
			value -= (2.25f / 2.75f);         
			return end * (7.5625f * (value) * value + .9375f) + start;
		}
		else
		{
			value -= (2.625f / 2.75f);        
			return end * (7.5625f * (value) * value + .984375f) + start;  
		}
	}

	static public float EaseInBack(float start, float end, float value)
	{
    
		end -= start;
      
		value /= 1;

		float s = 1.70158f;
      
		return end * (value) * value * ((s + 1) * value - s) + start;

	}


	static public float EaseOutBack(float start, float end, float value)
	{
    
		float s = 1.70158f;
    
		end -= start;
      
		value = (value / 1) - 1;
		return end * ((value) * value * ((s + 1) * value + s) + 1) + start;

	}

	static public float EaseInOutBack(float start, float end, float value)
	{

     	float s = 1.70158f;
	 	
     	end -= start;
	 	
     	value /= .5f;
	 	
     	if ((value) < 1){
	 	
     	        s *= (1.525f);
	 	
     	        return end / 2 * (value * value * (((s) + 1) * value - s)) + start;
	 	
     	}
	 	
     	value -= 2;
	 	
     	s *= (1.525f);
	 	
     	return end / 2 * ((value) * value * (((s) + 1) * value + s) + 2) + start;

        
	}

	static public float Punch(float amplitude, float value)
	{ 
		float s = 9;

		if (value == 0)
		{  
			return 0;  
		}

               
		if (value == 1)
		{         
			return 0;
		}

		float period = 1 * 0.3f;
		s = period / (2 * Mathf.PI) * Mathf.Asin(0);
		return (amplitude * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * 1 - s) * (2 * Mathf.PI) / period));

    }

	static public Vector3 GetBezierPosition(Vector3 start, Vector3 pass, Vector3 end, float delta)
	{
		return start * (1.0f - delta) * (1.0f - delta) + pass * 2 * delta * (1.0f - delta) + end * (delta * delta);
	}

	
    // 캣멀롬(CatmullRom Spline) 스플라인 곡선
    public static Vector3 CatmullRom(float t , Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return 0.5f * (( 2.0f * p1 ) + (-p0 + p2) * t + (2.0f * p0 - 5.0f * p1 + 4 * p2 - p3 )*t2 +
            (-p0 + 3.0f * p1 - 3.0f * p2 + p3)*t3 );
    }

    //  1차 베지어 곡선
    public static Vector3 BezierLinear(float t, Vector3 P0, Vector3 P1 )
    {
        return (1 - t) * P0 + t * P1;
    }

    // 2차 베지어 곡선
    public static Vector3 BezierQuadratic( float t, Vector3 P0, Vector3 P1, Vector3 P2 )
    {
        float t2 = (1-t) * (1-t);
        return t2 * P0 + 2 * t * (1 - t) * P1 + t * t * P2;
    }

    // 3차 베지어 곡선
    public static Vector3 BezierCubic(float t, Vector3 P0, Vector3 P1, Vector3 P2,  Vector3 P3 )
    {
        float t3 = Mathf.Pow((1 - t), 3);
        float t2 = Mathf.Pow((1 - t), 2);
        float t1 = 1 - t;

        return P0 * t3 + (3 * P1 * t * t2) + (3 * P2 * t * t * (1 - t)) + P3 * t * t * t;
    }

    // 4차 베지어 곡선
    public static Vector3 BezierQuartic(float t, Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, Vector3 P4)
    {
        
        float t4 = Mathf.Pow((1 - t), 4);
        float t3 = Mathf.Pow((1 - t), 3);
        float t2 = Mathf.Pow((1 - t), 2);
        float t1 = 1 - t;

        return P0 * t4 +
                4 * P1 * t * t3 +
                4 * P2 * t * t * t2 +
                4 * P3 * t * t * t * t1 +
                P4 * t * t * t * t;
                
    }


}
