define([
    'jquery',
    'loader'
], function ($) {
    'use strict';

    $.fn.movieCmts = (cmtUrl) => {
        const elements = {
            $section: $('#shortCmt-section'),
            $list: $('.commentPlace'),
            $pager: $('.cmtPageSelector'),
            $average: $('.cmts-average'),
            $amount: $('.cmts-amount')
        }

        const loadCmts = (page = 1) => {
            $.ajax({
                type: 'GET',
                url: cmtUrl + 'page/' + page,
                dataType: 'json',
            }).done(function (response) {
                if(page === 1) {
                    elements.$average.text(response.total_ratings_average_note);
                    elements.$amount.text(response.total_cmts);
                    Pagination.Init(document.getElementById('cmts-pager'), {
                        size: response.total_page_number,
                        page: 1,
                        step: 2
                    });
                }
                elements.$list.html('');
                $.each(response.cmts, function(i, v) { renderCmts(i, v); });
                elements.$section.trigger('processStop');
            }).fail(function (response) {
                elements.$section.trigger('processStop');
            });
        }

        const renderCmts = (id, cmt) => {
            let cmtMarkup = 
            `<div class="commentPlace">
            <div class="commentPlaceTop">
                <ul>
                    <li>${ cmt.member_name }</li>
                    <li><span class="bi bi-star-fill"></span>
                    <span>${ cmt.rating }</span> / 5</li>
                    <li>${ cmt.created_time.slice(0, 10) }</li>
                </ul>
            </div>                                                        
            <b>${ cmt.title }</b>
        </div>`;

            $('.commentPlace').append(cmtMarkup);
        };

        const Pagination = {
            code: '',
            
            Extend: function(data) {
                data = data;
                Pagination.size = data.size;
                Pagination.page = data.page;
                Pagination.step = data.step;
            },

            Add: function(s, f) {
                for (let i = s; i < f; i++) {
                    Pagination.code += '<a>' + i + '</a>';
                }
            },

            Click: function() {
                Pagination.page = +this.innerHTML;
                loadCmts(Pagination.page);
                Pagination.Start();
            },

            Prev: function() {
                Pagination.page--;
                if (Pagination.page < 1) {
                    Pagination.page = 1;
                }
                loadCmts(Pagination.page);
                Pagination.Start();
            },

            Next: function() {
                Pagination.page++;
                if (Pagination.page > Pagination.size) {
                    Pagination.page = Pagination.size;
                }
                loadCmts(Pagination.page);
                Pagination.Start();
            },

            Bind: function() {
                let a = Pagination.e.getElementsByTagName('a');
                for (let i = 0; i < a.length; i++) {
                    if (+a[i].innerHTML === Pagination.page) a[i].className = 'current';
                    a[i].addEventListener('click', Pagination.Click, false);
                }
            },

            Finish: function() {
                Pagination.e.innerHTML = Pagination.code;
                if(Pagination.page === 1) {
                    $('a.action.prev').hide();
                } else {
                    $('a.action.prev').show();
                }
                if(Pagination.page === Pagination.size) {
                    $('a.action.next').hide();
                } else {
                    $('a.action.next').show();
                }
                Pagination.code = '';
                Pagination.Bind();
            },

            Start: function() {
                if (Pagination.size < Pagination.step * 2 + 1) {
                    Pagination.Add(1, Pagination.size + 1);
                }
                else if (Pagination.page < Pagination.step * 2 + 1) {
                    Pagination.Add(1, Pagination.step * 2 + 2);
                }
                else if (Pagination.page > Pagination.size - Pagination.step * 2 + 1) {
                    Pagination.Add(Pagination.size - Pagination.step * 2, Pagination.size + 1);
                }
                else {
                    Pagination.Add(Pagination.page - Pagination.step, Pagination.page + Pagination.step + 1);
                }
                Pagination.Finish();
            },

            Buttons: function(e) {
                let nav = e.getElementsByTagName('a');
                nav[0].addEventListener('click', Pagination.Prev, false);
                nav[1].addEventListener('click', Pagination.Next, false);
            },

            Create: function(e) {
                let html = [
                    '<a class="action prev"><i class="bi bi-arrow-left"></i></a>',
                    '<span></span>',
                    '<a class="action next"><i class="bi bi-arrow-right"></i></a>'
                ];

                e.innerHTML = html.join('');
                Pagination.e = e.getElementsByTagName('span')[0];
                Pagination.Buttons(e);
            },

            Init: function(e, data) {
                Pagination.Extend(data);
                Pagination.Create(e);
                Pagination.Start();
            }
        };

        elements.$section.trigger('processStart');
        loadCmts();
    };
});
