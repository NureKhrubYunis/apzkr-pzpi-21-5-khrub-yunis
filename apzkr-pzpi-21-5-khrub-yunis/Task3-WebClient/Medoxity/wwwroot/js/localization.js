$(document).ready(function () {
    i18next.init({
        lng: 'en', // Язык по умолчанию
        resources: {
            en: {
                translation: {
                    "title": "Drug Details",
                    "loading": "Loading...",
                    "rating": "Rating",
                    "comments": "Comments",
                    "add_comment": "Add Comment",
                    "comment_text": "Comment text",
                    "add_rating": "Add Rating",
                    "description": "Description",
                    "manufacturer": "Manufacturer",
                    "price": "Price",
                    "stock_quantity": "Stock Quantity",
                    "release_date": "Release Date",
                    "your_rating": "Your rating",
                    "no_rating": "No rating yet. Add your rating below:"
                }
            },
            uk: {
                translation: {
                    "title": "Деталі препарату",
                    "loading": "Завантаження...",
                    "rating": "Рейтинг",
                    "comments": "Коментарі",
                    "add_comment": "Додати коментар",
                    "comment_text": "Текст коментаря",
                    "add_rating": "Додати рейтинг",
                    "description": "Опис",
                    "manufacturer": "Виробник",
                    "price": "Ціна",
                    "stock_quantity": "Кількість на складі",
                    "release_date": "Дата випуску",
                    "your_rating": "Ваш рейтинг",
                    "no_rating": "Рейтингу ще немає. Додайте свій рейтинг нижче:"
                }
            }
        }
    }, function (err, t) {
        jqueryI18next.init(i18next, $);
        $('body').localize();
    });

    $('#languageSelector').change(function () {
        var selectedLanguage = $(this).val();
        i18next.changeLanguage(selectedLanguage, function (err, t) {
            $('body').localize();
            loadContent(); // Перезагрузка контента на выбранном языке
        });
    });

    function loadContent() {
        var urlParams = new URLSearchParams(window.location.search);
        var drugID = urlParams.get('DrugID');

        if (drugID) {
            // Fetch drug details
            $.ajax({
                url: '/api/Drugs/drugs/' + drugID,
                method: 'GET',
                contentType: 'application/json',
                success: function (drug) {
                    var drugHtml = `
                        <h3 class="drug-name">${drug.drugName}</h3>
                        <p class="drug-description" data-i18n="description">${drug.description}</p>
                        <p><strong data-i18n="manufacturer">Manufacturer:</strong> ${drug.manufacturer}</p>
                        <p><strong data-i18n="price">Price:</strong> $${drug.price.toFixed(2)}</p>
                        <p><strong data-i18n="stock_quantity">Stock Quantity:</strong> ${drug.stockQuantity}</p>
                        <p><strong data-i18n="release_date">Release Date:</strong> ${new Date(drug.releaseDate).toLocaleDateString()}</p>
                    `;
                    $('#drugDetails').html(drugHtml);
                    $('body').localize(); // Apply translations
                },
                error: function () {
                    $('#drugDetails').html('<p data-i18n="loading">Error loading drug details.</p>');
                    $('body').localize(); // Apply translations
                }
            });

            // Fetch rating
            $.ajax({
                url: '/api/Rating/check-rating/' + drugID,
                method: 'GET',
                contentType: 'application/json',
                success: function (rating) {
                    console.log('Rating data:', rating); // Debugging line
                    if (rating && rating.value) {
                        $('#ratingSection').html('<p data-i18n="your_rating">Your rating: ' + rating.value + '</p>');
                    } else if (rating.message === "Rating not found.") {
                        $('#ratingSection').html('<p data-i18n="no_rating">No rating yet. Add your rating below:</p>');
                        $('#addRatingForm').show();
                    } else {
                        $('#ratingSection').html('<p data-i18n="loading">Error loading rating.</p>');
                    }
                    $('body').localize(); // Apply translations
                },
                error: function (xhr) {
                    console.error('Error loading rating:', xhr.responseText); // Debugging line
                    $('#ratingSection').html('<p data-i18n="loading">Error loading rating.</p>');
                    $('body').localize(); // Apply translations
                }
            });

            // Fetch comments
            function loadComments() {
                $.ajax({
                    url: '/api/Comments/comments/drug/' + drugID,
                    method: 'GET',
                    contentType: 'application/json',
                    success: function (comments) {
                        if (comments.length === 0) {
                            $('#commentsSection').html('<p data-i18n="loading">No comments available for this drug.</p>');
                        } else {
                            var commentsHtml = '<ul class="list-group">';
                            comments.forEach(function (comment) {
                                commentsHtml += `
                                    <li class="list-group-item">
                                        <p><strong>${comment.username}</strong> <small>${new Date(comment.commentDate).toLocaleDateString()}</small></p>
                                        <p>${comment.text}</p>
                                    </li>`;
                            });
                            commentsHtml += '</ul>';
                            $('#commentsSection').html(commentsHtml);
                        }
                        $('body').localize(); // Apply translations
                    },
                    error: function () {
                        $('#commentsSection').html('<p data-i18n="loading">Error loading comments.</p>');
                        $('body').localize(); // Apply translations
                    }
                });
            }
            loadComments();

            // Add comment
            $('#addCommentForm').submit(function (event) {
                event.preventDefault();
                var commentText = $('#commentText').val();

                $.ajax({
                    url: '/api/Comments/comment',
                    method: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({ drugID: drugID, text: commentText }),
                    success: function (response) {
                        alert(response.message);
                        $('#commentText').val(''); // Clear the textarea after adding the comment
                        loadComments(); // Reload comments to show the new one
                    },
                    error: function (xhr) {
                        if (xhr.responseJSON && xhr.responseJSON.message) {
                            alert('Error: ' + xhr.responseJSON.message);
                        } else {
                            alert('Error adding comment.');
                        }
                    }
                });
            });
        } else {
            $('#drugDetails').html('<p data-i18n="loading">Invalid drug ID.</p>');
            $('#commentsSection').html('');
            $('body').localize(); // Apply translations
        }
    }

    loadContent(); // Initial load
});
