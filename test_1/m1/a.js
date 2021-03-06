// Generated by Haxe 4.1.3
(function ($global) { "use strict";
var Test = function() { };
Test.main = function() {
	console.log("src/Test.hx:5:","#START");
	var sm = new TestControl();
	sm.Run();
	console.log("src/Test.hx:8:","#END");
};
var TestControl = function() {
	this.m_callstack_level = 0;
	this.MAX_CALLSTACK = 10;
	var _g = [];
	var _g1 = 0;
	var _g2 = this.MAX_CALLSTACK;
	while(_g1 < _g2) {
		var i = _g1++;
		_g.push(null);
	}
	this.m_callstacks = _g;
};
TestControl.prototype = {
	Update: function() {
		while(true) {
			var bFirst = false;
			if(this.m_nextfunc != null) {
				this.m_curfunc = this.m_nextfunc;
				this.m_nextfunc = null;
				bFirst = true;
			}
			this.m_noWait = false;
			if(this.m_curfunc != null) {
				this.m_curfunc(bFirst);
			}
			if(!this.m_noWait) {
				break;
			}
		}
	}
	,Goto: function(func) {
		this.m_nextfunc = func;
	}
	,CheckState: function(func) {
		return this.m_curfunc == func;
	}
	,HasNextState: function() {
		return this.m_nextfunc != null;
	}
	,NoWait: function() {
		this.m_noWait = true;
	}
	,GoSubState: function(nextstate,returnstate) {
		if(this.m_callstack_level >= this.MAX_CALLSTACK - 1) {
			console.log("src/TestControl.hx:54:","CALL STACK OVERFLOW");
			return;
		}
		this.m_callstacks[this.m_callstack_level] = returnstate;
		this.m_callstack_level += 1;
		this.Goto(nextstate);
	}
	,ReturnState: function() {
		if(this.m_callstack_level <= 0) {
			console.log("src/TestControl.hx:64:","CALL STACK UNDERFLOW");
			return;
		}
		this.m_callstack_level -= 1;
		var nextstate = this.m_callstacks[this.m_callstack_level];
		this.Goto(nextstate);
	}
	,Start: function() {
		this.Goto($bind(this,this.S_START));
	}
	,IsEnd: function() {
		return this.CheckState($bind(this,this.S_END));
	}
	,Run: function() {
		var LOOPMAX = 100000;
		this.Start();
		var _g = 0;
		var _g1 = LOOPMAX;
		while(_g < _g1) {
			var loop = _g++;
			if(loop >= LOOPMAX - 1) {
				console.log("src/TestControl.hx:95:","UNEXPECTED");
			}
			this.Update();
			if(this.IsEnd()) {
				break;
			}
		}
	}
	,S_0000: function(bFirst) {
		if(bFirst) {
			console.log("src/TestControl.hx:113:","Hello World!");
		}
		if(!this.HasNextState()) {
			this.Goto($bind(this,this.S_0001));
		}
	}
	,S_0001: function(bFirst) {
		var sec = new Date().getSeconds();
		var n = sec % 5;
		this.m_val = n;
		if(n == 0) {
			this.Goto($bind(this,this.S_0002));
		} else if(n == 1) {
			this.Goto($bind(this,this.S_0003));
		} else if(n == 2) {
			this.Goto($bind(this,this.S_0004));
		} else if(n == 3) {
			this.Goto($bind(this,this.S_0005));
		} else {
			this.Goto($bind(this,this.S_0006));
		}
	}
	,S_0002: function(bFirst) {
		if(bFirst) {
			console.log("src/TestControl.hx:145:","Zero");
		}
		if(!this.HasNextState()) {
			this.Goto($bind(this,this.S_END));
		}
	}
	,S_0003: function(bFirst) {
		if(bFirst) {
			console.log("src/TestControl.hx:161:","First");
		}
		if(!this.HasNextState()) {
			this.Goto($bind(this,this.S_END));
		}
	}
	,S_0004: function(bFirst) {
		if(bFirst) {
			console.log("src/TestControl.hx:177:","Two");
		}
		if(!this.HasNextState()) {
			this.Goto($bind(this,this.S_END));
		}
	}
	,S_0005: function(bFirst) {
		if(bFirst) {
			console.log("src/TestControl.hx:193:","Three");
		}
		if(!this.HasNextState()) {
			this.Goto($bind(this,this.S_END));
		}
	}
	,S_0006: function(bFirst) {
		if(bFirst) {
			console.log("src/TestControl.hx:209:","" + this.m_val);
		}
		if(!this.HasNextState()) {
			this.Goto($bind(this,this.S_END));
		}
	}
	,S_END: function(bFirst) {
	}
	,S_START: function(bFirst) {
		this.Goto($bind(this,this.S_0000));
		this.NoWait();
	}
};
var haxe_iterators_ArrayIterator = function(array) {
	this.current = 0;
	this.array = array;
};
haxe_iterators_ArrayIterator.prototype = {
	hasNext: function() {
		return this.current < this.array.length;
	}
	,next: function() {
		return this.array[this.current++];
	}
};
var $_;
function $bind(o,m) { if( m == null ) return null; if( m.__id__ == null ) m.__id__ = $global.$haxeUID++; var f; if( o.hx__closures__ == null ) o.hx__closures__ = {}; else f = o.hx__closures__[m.__id__]; if( f == null ) { f = m.bind(o); o.hx__closures__[m.__id__] = f; } return f; }
$global.$haxeUID |= 0;
Test.main();
})(typeof window != "undefined" ? window : typeof global != "undefined" ? global : typeof self != "undefined" ? self : this);
