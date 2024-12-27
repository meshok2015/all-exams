import pickle
import gi

gi.require_version('Gtk', '3.0')

from gi.repository import Gtk

bld = Gtk.Builder()
bld.add_from_file('ui.glade')

wnd = bld.get_object('wnd')
cnv = bld.get_object('cnv')
clr = bld.get_object('clr')
adj = bld.get_object('adj');

strokes = []
button = False

def mouse_down(obj, evt):
	global button
	point = [evt.x, evt.y]
	color = clr.get_color()
	rgb = [color.red / 65535.0, color.green / 65535.0, color.blue / 65535.0]
	thick = adj.get_value()
	strokes.append({'color': rgb, 'thickness': thick, 'points': [point]})
	button = True

def mouse_up(obj, evt):
	global button
	button = False

def mouse_move(obj, evt):
	global button
	if button == True:
		strokes[-1]['points'].append([evt.x, evt.y])
		cnv.queue_draw()

def update(obj, cr):
	cr.set_source_rgb(1.0, 1.0, 1.0)
	cr.paint()
	for stroke in strokes:
		color = stroke['color']
		cr.set_source_rgb(color[0], color[1], color[2])
		cr.set_line_width(stroke['thickness'])
		point = stroke['points'][0]
		cr.move_to(point[0], point[1])
		for i in range(1, len(stroke['points'])):
			point = stroke['points'][i]
			cr.line_to(point[0], point[1])
		cr.stroke()

def close_app(obj):
	f = open('save.pkl', 'wb')
	f.write(pickle.dumps(strokes))
	f.close()
	Gtk.main_quit()

try:
	f = open('save.pkl', 'rb')
	strokes = pickle.loads(f.read())
	f.close()
except:
	pass

tab = {
	'exit': close_app,
	'motion_notify_event': mouse_move,
	'button_press_event': mouse_down,
	'button_release_event': mouse_up,
	'cnv_draw': update
}
bld.connect_signals(tab)

wnd.show_all()

Gtk.main()
