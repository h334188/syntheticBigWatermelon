DECLARE_METHOD(int32_t, GetDeviceId, (void* motionEvent))
DECLARE_METHOD(int32_t, GetSource, (void* motionEvent))
DECLARE_METHOD(int32_t, GetAction, (void* motionEvent))
DECLARE_METHOD(int64_t, GetEventTimeMillis, (void* motionEvent))
DECLARE_METHOD(int64_t, GetDownTimeMillis, (void* motionEvent))
DECLARE_METHOD(int32_t, GetFlags, (void* motionEvent))
DECLARE_METHOD(int32_t, GetMetaState, (void* motionEvent))
DECLARE_METHOD(int32_t, GetActionButton, (void* motionEvent))
DECLARE_METHOD(int32_t, GetButtonState, (void* motionEvent))
DECLARE_METHOD(int32_t, GetClassification, (void* motionEvent))
DECLARE_METHOD(int32_t, GetEdgeFlags, (void* motionEvent))
DECLARE_METHOD(int32_t, GetPointerCount, (void* motionEvent))
DECLARE_METHOD(int32_t, GetPointerId, (void* motionEvent, int32_t pointerIndex))
DECLARE_METHOD(float, GetX, (void* motionEvent, int32_t pointerIndex))
DECLARE_METHOD(float, GetY, (void* motionEvent, int32_t pointerIndex))
DECLARE_METHOD(float, GetPressure, (void* motionEvent, int32_t pointerIndex))
DECLARE_METHOD(int32_t, GetPrecisionX, (void* motionEvent))
DECLARE_METHOD(int32_t, GetPrecisionY, (void* motionEvent))
DECLARE_METHOD(float, GetTouchMinor, (void* motionEvent, int32_t pointerIndex))
DECLARE_METHOD(float, GetTouchMajor, (void* motionEvent, int32_t pointerIndex))
DECLARE_METHOD(int32_t, GetHistorySize, (void* motionEvent))
DECLARE_METHOD(int64_t, GetHistoricalEventTimeMillis, (void* motionEvent, int32_t position))
DECLARE_METHOD(float, GetHistoricalAxisValue, (void* motionEvent, int32_t axis, int32_t pointerIndex, int32_t position))
DECLARE_METHOD(float, GetHistoricalPressure, (void* motionEvent, int32_t pointerIndex, int32_t position))
DECLARE_METHOD(float, GetHistoricalSize, (void* motionEvent, int32_t pointerIndex, int32_t position))
DECLARE_METHOD(float, GetHistoricalX, (void* motionEvent, int32_t pointerIndex, int position))
DECLARE_METHOD(float, GetHistoricalY, (void* motionEvent, int32_t pointerIndex, int position))
DECLARE_METHOD(float, GetHistoricalTouchMinor, (void* motionEvent, int32_t pointerIndex, int position))
DECLARE_METHOD(float, GetHistoricalTouchMajor, (void* motionEvent, int32_t pointerIndex, int position))
DECLARE_METHOD(float, GetOrientation, (void* motionEvent, int32_t pointerIndex))
DECLARE_METHOD(float, GetAxisValue, (void* motionEvent, int32_t axis, int32_t pointerIndex))
DECLARE_METHOD(float, GetSize, (void* motionEvent, int32_t pointerIndex))
DECLARE_METHOD(int32_t, GetToolType, (void* motionEvent, int32_t pointerIndex))
DECLARE_METHOD(void*, DeepClone, (void* motionEvent, void* (*allocate)(size_t)))