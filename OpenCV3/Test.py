import cv2
import OpenCVTool

# 定义显示窗口
cv2.namedWindow('video', cv2.WINDOW_NORMAL)
cv2.resizeWindow('video', 640, 480)

# 图片显示
# OpenCVTool.read_image('id=38274615.jpg', 'video')

# 视频显示
OpenCVTool.read_video('./洛奇主线截图MV_某光_新浪播客.mp4', 'video', 1000 // 30)

# 视频录制
# OpenCVTool.save_video('./洛奇主线截图MV_某光_新浪播客.mp4', 'mp4v', '录制1.mp4', 15, (320, 240))

cv2.destroyAllWindows()
