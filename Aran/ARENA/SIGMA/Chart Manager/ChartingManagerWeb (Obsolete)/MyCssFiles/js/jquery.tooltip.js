/*
 *	jquery.tooltip - v0.0.1
 *	Made by June Leoman Lapera
 *  Under MIT License
 * 
 *  jquery-boilerplate - v4.0.0
 *  A jump-start for jQuery plugins development.
 *  http://jqueryboilerplate.com
 *
 *  Made by Zeno Rocha
 *  Under MIT License
 */
// the semi-colon before function invocation is a safety net against concatenated
// scripts and/or other plugins which may not be closed properly.
;( function( $, window, document, undefined ) {

	"use strict";

	// undefined is used here as the undefined global variable in ECMAScript 3 is
	// mutable (ie. it can be changed by someone else). undefined isn't really being
	// passed in so we can ensure the value of it is truly undefined. In ES5, undefined
	// can no longer be modified.

	// window and document are passed through as local variable rather than global
	// as this (slightly) quickens the resolution process and can be more efficiently
	// minified (especially when both are regularly referenced in your plugin).

	// Create the defaults once
	var pluginName = "tooltip",
		defaults = {
			position: "",
			contentBGColor: "",
			labelColor: ""
		};

	// The actual plugin constructor
	function Tooltip ( element, options ) {
		this.element = element;

		// jQuery has an extend method which merges the contents of two or
		// more objects, storing the result in the first object. The first object
		// is generally empty as we don't want to alter the default options for
		// future instances of the plugin
		this.settings = $.extend( {}, defaults, options );
		this._defaults = defaults;
		this._name = pluginName;
		this.init();
	}

	// Avoid Tooltip.prototype conflicts
	$.extend( Tooltip.prototype, {
		init: function() {
			// Place initialization logic here
			// You already have access to the DOM element and
			// the options via the instance, e.g. this.element
			// and this.settings
			// you can add more functions like the one below and
			// call them like the example bellow
			this.tooltip = $(this.element);
			this.content = $('<span class="tooltip-content"></span>');
			this.triangle = $('<svg class="tooltip-triangle" width="12px" height="8px" viewBox="0 0 12 8"><polygon points="0 0 12 0 6 8"></polygon></svg>');
			this.render();
		},
		render: function() {
			var settings = this.settings;

			// set the position
			if (settings.position)
				this.setPosition(settings.position);

			// set content background color
			if (settings.contentBGColor)
				this.setColor(settings.contentBGColor);

			// set tooltip label color
			if (settings.labelColor)
				this.setLabelColor(settings.labelColor);

			// append triangle to content
			this.tooltip.append(this.triangle);

			// set tooltip color
			this.triangle.css('fill', settings.contentBGColor);

			// get and append the content to tooltip element
			var content = this.getContent();
			this.content.text(content);
			this.tooltip.append(this.content);
		},
		getContent: function() {
			return $(this.tooltip).data('content') || 'Please set data-content on tooltip element.';
		},
		setPosition: function(position) {
			if (typeof position === 'string') {
				this.tooltip.addClass(position);
			}
		},
		setColor: function(contentBGColor) {
			if (typeof contentBGColor === 'string') {
				this.content.css('background-color', contentBGColor);
			}
		},
		setLabelColor: function(labelColor) {
			if (typeof labelColor === 'string') {
				this.tooltip.css('color', labelColor);
			}
		}
	} );

	// A really lightweight plugin wrapper around the constructor,
	// preventing against multiple instantiations
	$.fn[ pluginName ] = function( options ) {
		return this.each( function() {
			if ( !$.data( this, "plugin_" + pluginName ) ) {
				$.data( this, "plugin_" +
					pluginName, new Tooltip( this, options ) );
			}
		} );
	};

} )( jQuery, window, document );